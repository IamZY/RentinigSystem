<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.community" @change="getMessages" placeholder="小区名"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.username" @change="getMessages" placeholder="留言用户"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.message" @change="getMessages" placeholder="留言内容"></el-input>
                </el-form-item>
                <el-form-item>
                    <div class="block">
                        <!--                        <span class="demonstration">默认</span>-->
                        <el-date-picker
                                v-model="filters.time"
                                @change="getMessages"
                                type="date"
                                placeholder="选择日期">
                        </el-date-picker>
                    </div>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" v-on:click="getMessages">查询</el-button>
                </el-form-item>
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="messages" highlight-current-row v-loading="listLoading" @selection-change="selsChange" style="width: 100%">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60">
            </el-table-column>

            <el-table-column prop="hmid" v-if="false">
            </el-table-column>
            <el-table-column prop="community" label="小区名" width="200" sortable>
            </el-table-column>
            <el-table-column prop="floor" label="楼层" width="250" sortable>
            </el-table-column>
            <el-table-column prop="area" label="面积" width="200" sortable>
            </el-table-column>
            <el-table-column prop="message" label="留言" width="200" sortable>
            </el-table-column>
            <el-table-column prop="username" label="留言用户" width="200" sortable>
            </el-table-column>
            <el-table-column prop="time" label="留言时间" min-width="200" sortable>
            </el-table-column>
            <el-table-column label="操作" width="200">
                <template scope="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>

        <!--工具条-->
        <el-col :span="24" class="toolbar">
            <el-button type="danger" @click="batchRemove" :disabled="this.sels.length===0">批量删除</el-button>
            <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="pageSize" :total="total" style="float:right;">
            </el-pagination>
        </el-col>

        <!--编辑界面-->
        <el-dialog title="编辑" v-model="editFormVisible" :close-on-click-modal="false">
            <el-form :model="editForm" label-width="80px" :rules="editFormRules" ref="editForm">

                <el-form-item label="hmid" prop="hmid" v-show="false">
                    <el-input v-model="editForm.hmid" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="小区名" prop="community">
                    <el-input v-model="editForm.community" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="留言" prop="message">
                    <el-input v-model="editForm.message"  auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="面积" prop="area">
                    <el-input v-model="editForm.area" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="楼层" prop="floor">
                    <el-input v-model="editForm.floor" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="留言用户" prop="username">
                    <el-input v-model="editForm.username" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>
<!--                hmid: 0,-->
<!--                community:'',-->
<!--                floor:'',-->
<!--                area:'',-->
<!--                message:'',-->
<!--                username:'',-->
<!--                time:''-->


                <el-form-item label="留言时间" prop="time">
                    <el-input v-model="editForm.time" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click.native="editFormVisible = false">取消</el-button>
                <el-button type="primary" @click.native="editSubmit" :loading="editLoading">提交</el-button>
            </div>
        </el-dialog>


    </section>
</template>

<script>
    import util from '../../common/js/util'
    // import axios from 'axios';
    //import NProgress from 'nprogress'
    import { getMessagesListPage, removeMessage, batchRemoveUser, editMessage, addUser } from '../../api/api';

    export default {
        data() {
            return {
                filters: {
                    community: '',
                    message:'',
                    username: '',
                    time: '',
                },
                messages: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 20,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    message: [
                        { required: true, message: '请输入留言' }
                    ]
                },
                //编辑界面数据
                editForm: {
                    // id: 0,
                    hmid: 0,
                    community:'',
                    floor:'',
                    area:'',
                    message:'',
                    username:'',
                    time:''
                },

                addFormVisible: false,//新增界面是否显示
                addLoading: false,
                addFormRules: {
                    message: [
                        { required: true, message: '请输入留言' }
                    ]
                },
                //新增界面数据
                addForm: {
                    username:'',
                    password: '',
                    name:'',
                    sex: '',
                    phone: '',
                }

            }
        },
        methods: {
            handleCurrentChange(val) {
                this.page = val;
                this.getMessages();
            },
            //获取用户列表
            getMessages() {
                // axios.get('http://localhost:4313/Admin/getUser').then((res)=>{
                //     console.log(res)
                // });
                var para = {
                    page: this.page,
                    message: this.filters.message,
                    username: this.filters.username,
                    time: this.filters.time,
                    community: this.filters.community,
                    pageSize: this.pageSize
                };
                this.listLoading = true;
                //NProgress.start();
                getMessagesListPage(para).then((res) => {
                    console.log(res)
                    // console.log(res.data.length)
                    this.total = res.data.total;
                    this.messages = res.data.entity;
                    this.listLoading = false;
                    //NProgress.done();
                });
            },
            //删除
            handleDel: function (index, row) {
                // console.log(index,row)
                this.$confirm('确认删除该记录吗?', '提示', {
                    type: 'warning'
                }).then(() => {
                    this.listLoading = true;
                    //NProgress.start();
                    var para = { hmid: row.hmid };
                    removeMessage(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getMessages();
                    });
                }).catch(() => {

                });
            },
            //显示编辑界面
            handleEdit: function (index, row) {
                this.editFormVisible = true;
                // _row = row
                // row.sex = row.sex == '男' ? 0 : 1
                // console.log(row)
                this.editForm = Object.assign({}, row);
            },
            //显示新增界面
            handleAdd: function () {
                this.addFormVisible = true;
                this.addForm = {
                    name: '',
                    sex: -1,
                    age: 0,
                    birth: '',
                    addr: ''
                };
            },
            //编辑
            editSubmit: function () {
                this.$refs.editForm.validate((valid) => {
                    if (valid) {
                        this.$confirm('确认提交吗？', '提示', {}).then(() => {
                            this.editLoading = true;
                            //NProgress.start();
                            //let para = Object.assign({}, this.editForm);
                            var para = {
                                hmid: this.editForm.hmid,
                                message: this.editForm.message,
                            };
                            // console.log(para)

                            // para.birth = (!para.birth || para.birth == '') ? '' : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
                            editMessage(para).then((res) => {
                                this.editLoading = false;
                                //NProgress.done();

                                // console.log(res)

                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['editForm'].resetFields();
                                this.editFormVisible = false;
                                this.getMessages();
                            });
                        });
                    }
                });
            },
            //新增
            addSubmit: function () {
                this.$refs.addForm.validate((valid) => {
                    if (valid) {
                        this.$confirm('确认提交吗？', '提示', {}).then(() => {
                            this.addLoading = true;
                            //NProgress.start();
                            // let para = Object.assign({}, this.addForm);
                            var para = {
                                username: this.addForm.username,
                                password: this.addForm.password,
                                name: this.addForm.name,
                                sex: this.addForm.sex,
                                phone: this.addForm.phone
                            };
                            addUser(para).then((res) => {
                                this.addLoading = false;
                                //NProgress.done();
                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['addForm'].resetFields();
                                this.addFormVisible = false;
                                this.getMessages();
                            });
                        });
                    }
                });
            },
            selsChange: function (sels) {
                this.sels = sels;
            },
            //批量删除
            batchRemove: function () {
                var ids = this.sels.map(item => item.id).toString();
                this.$confirm('确认删除选中记录吗？', '提示', {
                    type: 'warning'
                }).then(() => {
                    this.listLoading = true;
                    //NProgress.start();
                    let para = { ids: ids };
                    batchRemoveUser(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getMessages();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getMessages();
        }
    }

</script>

<style scoped>

</style>