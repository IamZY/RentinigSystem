<template>
    <section>
        <!--工具条-->
        <el-col :span="24"  style="padding: 10px 10px"></el-col>

        <!--列表-->
        <el-table :data="auth" highlight-current-row v-loading="listLoading" @selection-change="selsChange"
                  style="width: 100%;">
            <el-table-column type="selection" width="55">
            </el-table-column>
<!--            <el-table-column type="index" width="60">-->
<!--            </el-table-column>-->

            <el-table-column prop="uaid" label="序号" width="120">
            </el-table-column>

            <el-table-column prop="username" label="用户名" width="120" sortable>
            </el-table-column>
            <el-table-column label="首页权限" width="120" sortable>
                <template slot-scope="scope">
                    <el-switch
                            v-model="scope.row.main"
                            on-color="#13ce66"
                            off-color="#ff4949"
                            on-value="1"
                            off-value="0"
                            @change="changeSwitch($event,scope.row)"
                    >
                    </el-switch>
                </template>

            </el-table-column>
            <el-table-column prop="users" label="用户权限" width="120" sortable>
                <template slot-scope="scope">
                    <el-switch
                            v-model="scope.row.users"
                            on-color="#13ce66"
                            off-color="#ff4949"
                            on-value="1"
                            off-value="0"
                            @change="changeSwitch($event,scope.row)"
                    >
                    </el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="house123" label="房屋权限" width="120" sortable>
                <template slot-scope="scope">
                    <el-switch
                            v-model="scope.row.house"
                            on-color="#13ce66"
                            off-color="#ff4949"
                            on-value="1"
                            off-value="0"
                            @change="changeSwitch($event,scope.row)"
                    >
                    </el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="orders" label="订单权限" width="120" sortable>
                <template slot-scope="scope">
                    <el-switch
                            v-model="scope.row.orders"
                            on-color="#13ce66"
                            off-color="#ff4949"
                            on-value="1"
                            off-value="0"
                            @change="changeSwitch($event,scope.row)"
                    >
                    </el-switch>
                </template>
            </el-table-column>
            <el-table-column prop="others" label="其他权限" min-width="180" sortable>
                <template slot-scope="scope">
                    <el-switch
                            v-model="scope.row.others"
                            on-color="#13ce66"
                            off-color="#ff4949"
                            on-value="1"
                            off-value="0"
                            @change="changeSwitch($event,scope.row)"
                    >
                    </el-switch>
                </template>
            </el-table-column>
<!--            <el-table-column label="操作" width="150">-->
<!--                <template scope="scope">-->
<!--&lt;!&ndash;                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>&ndash;&gt;-->
<!--&lt;!&ndash;                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">删除</el-button>&ndash;&gt;-->
<!--                </template>-->
<!--            </el-table-column>-->
        </el-table>

        <!--工具条-->
        <el-col :span="24" class="toolbar">
<!--            <el-button type="danger" @click="batchRemove" :disabled="this.sels.length===0">批量删除</el-button>-->
            <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="pageSize"
                           :total="total" style="float:right;">
            </el-pagination>
        </el-col>

    </section>
</template>

<script>
    import util from '../../common/js/util'
    // import axios from 'axios';
    //import NProgress from 'nprogress'
    import { getUserAuthListPage, updateAuth } from '../../api/api';

    export default {
        data() {
            return {
                // value: false,
                filters: {
                    username: '',
                    name: '',
                    password: '',
                    sex: '',
                    phone: '',
                },
                auth: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 20,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    name: [
                        {required: true, message: '请输入姓名', trigger: 'blur'}
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
                }

            }
        },
        methods: {
            changeSwitch: function(data,row) {
                // console.log(data)
                // console.log(row)
                var para = {
                    house: row.house,
                    main: row.main,
                    uaid: row.uaid,
                    users: row.users,
                    orders: row.orders,
                    others: row.others,
                    uid: row.uid
                }

                updateAuth(para).then((res) => {
                    this.getUserAuths();
                });

            },
            //性别显示转换
            formatSex: function (row, column) {
                return row.sex == 1 ? '男' : row.sex == 0 ? '女' : '未知';
            },
            handleCurrentChange(val) {
                this.page = val;
                this.getUserAuths();
            },
            //获取用户列表
            getUserAuths() {
                // axios.get('http://localhost:4313/Admin/getUser').then((res)=>{
                //     console.log(res)
                // });
                var para = {
                    page: this.page,
                    name: this.filters.name,
                    // username: this.filters.username,
                    // password: this.filters.password,
                    // phone: this.filters.phone,
                    // sex: this.filters.sex,
                    pageSize: this.pageSize
                };
                this.listLoading = true;
                //NProgress.start();
                getUserAuthListPage(para).then((res) => {
                    console.log(res)
                    // console.log(res.data.length)
                    this.total = res.data.total;
                    this.auth = res.data.users;
                    console.log(this.auth)
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
                        this.getUserAuths();
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
                                phone: this.editForm.phone
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
                                this.getUserAuths();
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
                                this.getUserAuths();
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
                        this.getUserAuths();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getUserAuths();
        }
    }

</script>

<style scoped>

</style>