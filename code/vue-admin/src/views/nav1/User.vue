<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.username" @change="getUsers" placeholder="用户名"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.password" @change="getUsers"  placeholder="密码"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.name" @change="getUsers" placeholder="姓名"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.sex" @change="getUsers" placeholder="性别"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.phone" @change="getUsers" placeholder="手机号"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-select v-model="filters.isSys" @change="getUsers" placeholder="是否系统用户">
                        <el-option value="" label="全部"></el-option>
                        <el-option value="1" label="是"></el-option>
                        <el-option value="0" label="否"></el-option>
                    </el-select>

                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.phone" @change="getUsers" placeholder="手机号"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" v-on:click="getUsers">查询</el-button>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="handleAdd">新增</el-button>
                </el-form-item>
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="users" highlight-current-row v-loading="listLoading" @selection-change="selsChange"
                  style="width: 100%;">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60">
            </el-table-column>

            <el-table-column prop="uid" v-if="false">
            </el-table-column>
            <el-table-column prop="uiid" v-if="false">
            </el-table-column>

            <el-table-column prop="username" label="用户名" width="120" sortable>
            </el-table-column>
            <el-table-column prop="password" label="密码" width="100" sortable>
                <!--                <el-table-column prop="password" label="密码" width="100" :formatter="formatSex" sortable>-->
            </el-table-column>
            <el-table-column prop="name" label="姓名" width="120" sortable>
            </el-table-column>
            <el-table-column prop="sex" label="性别" width="120" sortable>
            </el-table-column>
            <el-table-column prop="phone" label="电话" min-width="180" sortable>
            </el-table-column>

            <el-table-column prop="isSys" label="是否系统用户" min-width="180" sortable>

                <template slot-scope="scope">
                    <el-tag
                            type="success" v-if="scope.row.isSys == '1'"
                            disable-transitions>是</el-tag>
                    <el-tag
                            type="dager" v-if="scope.row.isSys == '0'"
                            disable-transitions>否</el-tag>
                </template>
            </el-table-column>

            <el-table-column label="操作" width="150">
                <template scope="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">删除</el-button>
                </template>
            </el-table-column>
        </el-table>

        <!--工具条-->
        <el-col :span="24" class="toolbar">
            <el-button type="danger" @click="batchRemove" :disabled="this.sels.length===0">批量删除</el-button>
            <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="pageSize"
                           :total="total" style="float:right;">
            </el-pagination>
        </el-col>

        <!--编辑界面-->
        <el-dialog title="编辑" v-model="editFormVisible" :close-on-click-modal="false">
            <el-form :model="editForm" label-width="80px" :rules="editFormRules" ref="editForm">
                <el-form-item label="用户名" prop="username">
                    <el-input v-model="editForm.username" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="password">
                    <el-input v-model="editForm.password" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="姓名" prop="name">
                    <el-input v-model="editForm.name" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="性别" prop="sex">
                    <el-radio-group v-model="editForm.sex">
                        <el-radio class="radio" label="男">男</el-radio>
                        <el-radio class="radio" label="女">女</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="手机号" prop="phone">
                    <el-input v-model="editForm.phone" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="是否系统用户" prop="isSys">
                    <el-select v-model="editForm.isSys">
                        <el-option value="1" label="是"></el-option>
                        <el-option value="0" label="否"></el-option>
                    </el-select>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click.native="editFormVisible = false">取消</el-button>
                <el-button type="primary" @click.native="editSubmit" :loading="editLoading">提交</el-button>
            </div>
        </el-dialog>

        <!--新增界面-->
        <el-dialog title="新增" v-model="addFormVisible" :close-on-click-modal="false">
            <el-form :model="addForm" label-width="80px" :rules="addFormRules" ref="addForm">
                <el-form-item label="用户名" prop="username">
                    <el-input v-model="addForm.username" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="password">
                    <el-input v-model="addForm.password" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="姓名" prop="name">
                    <el-input v-model="addForm.name" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="性别" prop="sex">
                    <el-radio-group v-model="addForm.sex">
                        <el-radio class="radio" label="男">男</el-radio>
                        <el-radio class="radio" label="女">女</el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item label="是否系统用户" prop="isSys">
                    <el-select v-model="addForm.isSys">
                        <el-option value="1" label="是"></el-option>
                        <el-option value="0" label="否"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="手机号" prop="phone">
                    <el-input v-model="addForm.phone" auto-complete="off"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click.native="addFormVisible = false">取消</el-button>
                <el-button type="primary" @click.native="addSubmit" :loading="addLoading">提交</el-button>
            </div>
        </el-dialog>
    </section>
</template>

<script>
    import util from '../../common/js/util'
    // import axios from 'axios';
    //import NProgress from 'nprogress'
    import {getUserListPage, removeUser, batchRemoveUser, editUser, addUser} from '../../api/api';

    export default {
        data() {
            return {
                filters: {
                    username: '',
                    name: '',
                    password: '',
                    sex: '',
                    phone: '',
                    isSys: ''
                },
                users: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 20,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    username: [
                        {required: true, message: '请输入用户名', trigger: 'blur'}
                    ],
                    name: [
                        {required: true, message: '请输入姓名', trigger: 'blur'}
                    ],
                    password: [
                        {required: true, message: '请输入密码', trigger: 'blur'}
                    ],
                    phone: [
                        {required: true, message: '请输入手机号', trigger: 'blur'},
                        {
                            validator: function (rule, value, callback) {
                                if (/^1[34578]\d{9}$/.test(value) == false) {
                                    callback(new Error("请输入正确的手机号"));
                                } else {
                                    callback();
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    sex: [
                        {required: true, message: '请输入性别', trigger: 'blur'}
                    ]
                },
                //编辑界面数据
                editForm: {
                    // id: 0,
                    uid: 0,
                    uiid: 0,
                    username: '',
                    password: '',
                    name: '',
                    sex: '',
                    phone: '',
                    isSys: '',
                },

                addFormVisible: false,  //新增界面是否显示
                addLoading: false,
                addFormRules: {
                    username: [
                        {required: true, message: '请输入用户名', trigger: 'blur'}
                    ],
                    name: [
                        {required: true, message: '请输入姓名', trigger: 'blur'}
                    ],
                    password: [
                        {required: true, message: '请输入密码', trigger: 'blur'}
                    ],
                    phone: [
                        {required: true, message: '请输入手机号', trigger: 'blur'},
                        {
                            validator: function (rule, value, callback) {
                                if (/^1[34578]\d{9}$/.test(value) == false) {
                                    callback(new Error("请输入正确的手机号"));
                                } else {
                                    callback();
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    sex: [
                        {required: true, message: '请输入性别', trigger: 'blur'}
                    ]
                },
                //新增界面数据
                addForm: {
                    username: '',
                    password: '',
                    name: '',
                    sex: '',
                    phone: '',
                    isSys: '',
                }

            }
        },
        methods: {
            //性别显示转换
            formatSex: function (row, column) {
                return row.sex == 1 ? '男' : row.sex == 0 ? '女' : '未知';
            },
            handleCurrentChange(val) {
                this.page = val;
                this.getUsers();
            },
            //获取用户列表
            getUsers() {
                // axios.get('http://localhost:4313/Admin/getUser').then((res)=>{
                //     console.log(res)
                // });
                var para = {
                    page: this.page,
                    name: this.filters.name,
                    username: this.filters.username,
                    password: this.filters.password,
                    phone: this.filters.phone,
                    sex: this.filters.sex,
                    pageSize: this.pageSize,
                    isSys: this.filters.isSys
                };
                this.listLoading = true;
                //NProgress.start();
                getUserListPage(para).then((res) => {
                    // console.log(para)
                    // console.log(res.data.length)
                    this.total = res.data.total;
                    this.users = res.data.entity;
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
                    var para = {uid: row.uid, uiid: row.uiid};
                    removeUser(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getUsers();
                    });
                }).catch(() => {

                });
            },
            //显示编辑界面
            handleEdit: function (index, row) {
                this.editFormVisible = true;
                // _row = row
                // row.sex = row.sex == '男' ? 0 : 1
                console.log(row)
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
                                uid: this.editForm.uid,
                                uiid: this.editForm.uiid,
                                username: this.editForm.username,
                                password: this.editForm.password,
                                name: this.editForm.name,
                                sex: this.editForm.sex,
                                phone: this.editForm.phone,
                                isSys: this.editForm.isSys
                            };
                            // console.log(para)

                            // para.birth = (!para.birth || para.birth == '') ? '' : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
                            editUser(para).then((res) => {
                                this.editLoading = false;
                                //NProgress.done();

                                // console.log(res)

                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['editForm'].resetFields();
                                this.editFormVisible = false;
                                this.getUsers();
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
                                phone: this.addForm.phone,
                                isSys: this.addForm.isSys
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
                                this.getUsers();
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
                    let para = {ids: ids};
                    batchRemoveUser(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getUsers();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getUsers();
        }
    }

</script>

<style scoped>

</style>