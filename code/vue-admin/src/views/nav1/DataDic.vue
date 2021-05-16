<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.dkey" @change="getDataDic" placeholder="键名"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.dvalue" @change="getDataDic" placeholder="值1"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.dvalue2" @change="getDataDic" placeholder="值2"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.dvalue3" @change="getDataDic" placeholder="值3"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" v-on:click="getDataDic">查询</el-button>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="handleAdd">新增</el-button>
                </el-form-item>
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="DataDic" highlight-current-row v-loading="listLoading" @selection-change="selsChange" style="width: 100%">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60">
            </el-table-column>

            <el-table-column prop="did" v-if="false">
            </el-table-column>
            <el-table-column prop="dkey" label="键名" width="200" sortable>
            </el-table-column>
            <el-table-column prop="dvalue" label="值1" width="250" sortable>
            </el-table-column>
            <el-table-column prop="dvalue2" label="值2" width="200" sortable>
            </el-table-column>
            <el-table-column prop="dvalue3" label="值3" min-width="200" sortable>
            </el-table-column>
            <el-table-column prop="remark" label="备注说明" min-width="200" sortable>
            </el-table-column>
            <el-table-column label="操作" width="200">
                <template scope="scope">
                    <el-button type="primary" size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
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

                <el-form-item prop="did" v-model="editForm.did" v-if="false"></el-form-item>

                <el-form-item label="键名" prop="dkey">
                    <el-input v-model="editForm.dkey" auto-complete="off" readonly="readonly"></el-input>
                </el-form-item>
                <el-form-item label="值1" prop="dvalue">
                    <el-input v-model="editForm.dvalue" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="值2" prop="dvalue2">
                    <el-input v-model="editForm.dvalue2" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="值3" prop="dvalue3">
                    <el-input v-model="editForm.dvalue3" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="备注说明" prop="remark">
                    <el-input v-model="editForm.remark" auto-complete="off"></el-input>
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

                <el-form-item prop="did" v-model="editForm.did" v-if="false"></el-form-item>

                <el-form-item label="键名" prop="dkey">
                    <el-input v-model="addForm.dkey" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="值1" prop="dvalue">
                    <el-input v-model="addForm.dvalue" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="值2" prop="dvalue2">
                    <el-input v-model="addForm.dvalue2" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="值3" prop="dvalue3">
                    <el-input v-model="addForm.dvalue3" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="备注说明" prop="remark">
                    <el-input v-model="addForm.remark" auto-complete="off"></el-input>
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
    import { getDataDicListPage, removeDataDic, batchRemoveUser, editDataDic, addDataDic } from '../../api/api';

    export default {
        data() {
            return {
                filters: {
                    dkey: '',
                    dvalue: '',
                    dvalue2: '',
                    dvalue3: '',
                },
                DataDic: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 20,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    dkey: [
                        { required: true, message: '请输入姓名', trigger: 'blur' }
                    ]
                },
                //编辑界面数据
                editForm: {
                    // id: 0,
                    did: '',
                    dkey: '',
                    dvalue: '',
                    dvalue2: '',
                    dvalue3: '',
                    remark: '',
                },

                addFormVisible: false,//新增界面是否显示
                addLoading: false,
                addFormRules: {
                    dkey: [
                        { required: true, message: '请输入键名', trigger: 'blur' }
                    ]
                },
                //新增界面数据
                addForm: {
                    did: '',
                    dkey: '',
                    dvalue: '',
                    dvalue2: '',
                    dvalue3: '',
                    remark: '',
                }

            }
        },
        methods: {
            handleCurrentChange(val) {
                this.page = val;
                this.getDataDic();
            },
            //获取用户列表
            getDataDic() {
                // axios.get('http://localhost:4313/Admin/getUser').then((res)=>{
                //     console.log(res)
                // });
                var para = {
                    page: this.page,
                    dkey: this.filters.dkey,
                    dvalue: this.filters.dvalue,
                    dvalue2: this.filters.dvalue2,
                    dvalue3: this.filters.dvalue3,
                    pageSize: this.pageSize
                };
                this.listLoading = true;
                //NProgress.start();
                getDataDicListPage(para).then((res) => {
                    console.log(res)
                    // console.log(res.data.length)
                    this.total = res.data.total;
                    this.DataDic = res.data.datas;
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
                    var para = { did: row.did };
                    removeDataDic(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getDataDic();
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
                    did: 0,
                    dkey: '',
                    dvalue: '',
                    dvalue2: '',
                    dvalue3: '',
                    remark: '',
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
                                did: this.editForm.did,
                                dkey: this.editForm.dkey,
                                dvalue: this.editForm.dvalue,
                                dvalue2: this.editForm.dvalue2,
                                dvalue3: this.editForm.dvalue3,
                                remark: this.editForm.remark,
                            };
                            // console.log(para)

                            // para.birth = (!para.birth || para.birth == '') ? '' : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
                            editDataDic(para).then((res) => {
                                this.editLoading = false;
                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['editForm'].resetFields();
                                this.editFormVisible = false;
                                this.getDataDic();
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
                                did: this.addForm.did,
                                dkey: this.addForm.dkey,
                                dvalue: this.addForm.dvalue,
                                dvalue2: this.addForm.dvalue2,
                                dvalue3: this.addForm.dvalue3,
                                remark: this.addForm.remark
                            };
                            addDataDic(para).then((res) => {
                                this.addLoading = false;
                                //NProgress.done();
                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['addForm'].resetFields();
                                this.addFormVisible = false;
                                this.getDataDic();
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
                        this.getDataDic();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getDataDic();
        }
    }

</script>

<style scoped>

</style>