<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.oid" @change="getOrders" placeholder="订单号"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.community" @change="getOrders" placeholder="小区名"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.area" @change="getOrders" placeholder="面积"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.floor" @change="getOrders" placeholder="楼层"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.unitType" @change="getOrders" placeholder="户型"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.duration" @change="getOrders" placeholder="租赁时长"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.username" @change="getOrders" placeholder="租赁人"></el-input>
                </el-form-item>

                <el-form-item>
<!--                    <el-input v-model="filters.state" @change="getOrders" placeholder="订单状态"></el-input>-->
                    <el-select prop="state" v-model="filters.state" @change="getOrders">
                        <el-option label="全部" value=""></el-option>
                        <el-option label="已支付" value="1"></el-option>
                        <el-option label="已提交" value="0"></el-option>
                    </el-select>
                </el-form-item>

                <el-form-item>
<!--                    <el-input v-model="filters.time" @change="getOrders" placeholder="下单时间"></el-input>-->
                    <div class="block">
<!--                        <span class="demonstration">默认</span>-->
                        <el-date-picker
                                v-model="filters.time"
                                @change="getOrders"
                                type="date"
                                placeholder="选择日期">
                        </el-date-picker>
                    </div>
                </el-form-item>

                <el-form-item>
                    <el-button type="primary" v-on:click="getOrders">查询</el-button>
                </el-form-item>
<!--                <el-form-item>-->
<!--                    <el-button type="primary" @click="handleAdd">新增</el-button>-->
<!--                </el-form-item>-->
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="orders" highlight-current-row v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60">
            </el-table-column>

            <el-table-column prop="ouhid" v-if="false">
            </el-table-column>

            <el-table-column prop="oid" label="订单号" width="120"  sortable>
            </el-table-column>

            <el-table-column prop="community" label="小区名" width="150"  sortable>
            </el-table-column>

            <el-table-column prop="area" label="面积" width="150" sortable>
            </el-table-column>

            <el-table-column prop="unitType" label="户型" width="150" sortable>
            </el-table-column>

            <el-table-column prop="floor" label="楼层" width="150" sortable>
            </el-table-column>

            <el-table-column prop="duration" label="租赁时长" width="150" :formatter="formatDuration" sortable>
            </el-table-column>
            <el-table-column prop="rent" label="租金" width="120" :formatter="formatRent" sortable>
            </el-table-column>
            <el-table-column prop="state" label="订单状态" width="120" sortable>
                <template slot-scope="scope">
                    <el-tag
                            v-if="scope.row.state == '0'"
                            disable-transitions>已提交</el-tag>
                    <el-tag
                            type="success" v-if="scope.row.state == '1'"
                            disable-transitions>已支付</el-tag>
                </template>
            </el-table-column>
            <el-table-column prop="time" label="下单时间" width="200" sortable>
            </el-table-column>

            <el-table-column prop="name" label="租赁人姓名" width="150" sortable>
            </el-table-column>

            <el-table-column prop="phone" label="租赁人联系方式" width="200" sortable>
            </el-table-column>

            <el-table-column label="操作" width="150" fixed="right">
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
                <el-form-item label="ouhid" prop="ouhid" v-show="false">
                    <el-input v-model="editForm.ouhid" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="oid" prop="oid" v-show="false">
                    <el-input v-model="editForm.oid" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="小区名" prop="community">
                    <el-input v-model="editForm.community" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="面积" prop="area">
                    <el-input v-model="editForm.area" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="楼层" prop="floor">
                    <el-input v-model="editForm.floor" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="户型" prop="unitType">
                    <el-input v-model="editForm.unitType" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="租赁时长" prop="duration">
                    <el-input v-model="editForm.duration"  auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="租金" prop="rent">
                    <el-input v-model="editForm.rent" auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="租赁人" prop="name">
                    <el-input v-model="editForm.name" readonly="readonly"  auto-complete="off"></el-input>
                </el-form-item>

                <el-form-item label="支付状态" prop="state">
                    <!--                    <el-input v-model="filters.state" @change="getOrders" placeholder="订单状态"></el-input>-->
                    <el-select prop="state" v-model="editForm.state">
<!--                        <el-option label="全部" value=""></el-option>-->
                        <el-option label="已支付" value="1"></el-option>
                        <el-option label="已提交" value="0"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="提交日期" prop="time">
                    <el-input v-model="editForm.time" readonly="readonly" auto-complete="off"></el-input>
                </el-form-item>
<!--                <el-form-item label="提交日期" prop="time">-->
<!--                    &lt;!&ndash;                    <el-input v-model="filters.time" @change="getOrders" placeholder="下单时间"></el-input>&ndash;&gt;-->
<!--                    <div class="block">-->
<!--&lt;!&ndash;                        <span class="demonstration">默认</span>&ndash;&gt;-->
<!--                        <el-date-picker-->
<!--                                prop="time"-->
<!--                                v-model="editForm.time"-->
<!--                                type="date"-->
<!--                                placeholder="选择日期">-->
<!--                        </el-date-picker>-->
<!--                    </div>-->
<!--                </el-form-item>-->
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
    import { getOrdersListPage, removeOrder, batchRemoveUser, editOrder, addUser } from '../../api/api';

    export default {
        data() {
            return {
                filters: {
                    oid:'',
                    community: '',
                    area: '',
                    unitType: '',
                    duration: '',
                    rent: '',
                    name: '',
                    floor: '',
                    state: '',
                    time: '',
                },
                orders: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 20,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    duration: [
                        { required: true, message: '请输入租赁时长'  },
                        {
                            validator: function (rule, value, callback) {
                                let reg = /^[1-9]([0-9])*$/
                                let flag = reg.test(value);
                                if (!flag) {
                                    callback(new Error('必须整数'))
                                } else {
                                    callback()
                                }
                            }
                        }
                    ],
                    rent: [
                        { required: true, message: '请输入租金' },
                        {
                            validator: function (rule, value, callback) {
                                let reg = /^[1-9]([0-9])*$/
                                let flag = reg.test(value);
                                if (!flag) {
                                    callback(new Error('必须整数'))
                                } else {
                                    callback()
                                }
                            }
                        }
                    ],

                },
                //编辑界面数据
                editForm: {
                    ouhid:'',
                    oid:'',
                    community: '',
                    area: '',
                    unitType: '',
                    duration: '',
                    rent: '',
                    name: '',
                    floor: '',
                    state: '',
                    time: '',
                },

                addFormVisible: false,//新增界面是否显示
                addLoading: false,
                addFormRules: {
                    name: [
                        { required: true, message: '请输入姓名', trigger: 'blur' }
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
            //性别显示转换
            formatSex: function (row, column) {
                return row.sex == 1 ? '男' : row.sex == 0 ? '女' : '未知';
            },
            formatDuration: function(row, column) {
                return row.duration  + '个月';
            },
            formatRent: function(row, column) {
                return row.rent + '元';
            },
            handleCurrentChange(val) {
                this.page = val;
                this.getOrders();
            },
            //获取用户列表
            getOrders() {
                // axios.get('http://localhost:4313/Admin/getUser').then((res)=>{
                //     console.log(res)
                // });
                var para = {
                    page: this.page,
                    name: this.filters.name,
                    oid: this.filters.oid,
                    community: this.filters.community,
                    area: this.filters.area,
                    unitType: this.filters.unitType,
                    duration: this.filters.duration,
                    rent: this.filters.rent,
                    floor: this.filters.floor,
                    state: this.filters.state,
                    time: this.filters.time,
                    pageSize: this.pageSize
                };
                this.listLoading = true;
                //NProgress.start();
                getOrdersListPage(para).then((res) => {
                    console.log(res)
                    // console.log(res.data.length)
                    this.total = res.data.total;
                    this.orders = res.data.entity;
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
                    var para = { ouhid: row.ouhid };
                    removeOrder(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getOrders();
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
                this.$nextTick(()=>this.$refs.editForm.clearValidate())
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
                                ouhid: this.editForm.ouhid,
                                oid: this.editForm.oid,
                                community:  this.editForm.community,
                                area:  this.editForm.area,
                                unitType:  this.editForm.unitType,
                                duration: this.editForm.duration,
                                rent: this.editForm.rent,
                                name: this.editForm.name,
                                floor: this.editForm.floor,
                                state: this.editForm.state,

                            };
                            // console.log(para)

                            // para.birth = (!para.birth || para.birth == '') ? '' : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
                            editOrder(para).then((res) => {
                                this.editLoading = false;
                                //NProgress.done();

                                // console.log(res)

                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['editForm'].resetFields();
                                this.editFormVisible = false;
                                this.getOrders();
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
                                this.getOrders();
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
                        this.getOrders();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getOrders();
        }
    }

</script>

<style scoped>

</style>