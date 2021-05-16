<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.area" @change="getAudit" placeholder="房屋面积"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.price" @change="getAudit" placeholder="价格"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.pay" @change="getAudit" placeholder="付费方式"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.unitType" @change="getAudit" placeholder="户型"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.floor" @change="getAudit" placeholder="楼层"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.community" @change="getAudit" placeholder="小区名"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.region" @change="getAudit" placeholder="区县"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.city" @change="getAudit" placeholder="城市"></el-input>
                </el-form-item>

                <el-form-item>
                    <!--                    <el-input v-model="filters.type" @change="getHouses" placeholder="小区名"></el-input>-->
                    <el-select v-model="filters.type" placeholder="租房类型" @change="getAudit">
                        <el-option value="" label="全部"></el-option>
                        <el-option value="1" label="租房"></el-option>
                        <el-option value="2" label="新房"></el-option>
                        <el-option value="3" label="二手房"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" v-on:click="getAudit">查询</el-button>
                </el-form-item>
<!--                <el-form-item>-->
<!--                    <el-button type="primary" @click="handleAdd">新增</el-button>-->
<!--                </el-form-item>-->
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="audit"  highlight-current-row v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60" v-if="false">
            </el-table-column>

            <el-table-column prop="hid" align="center" label="房屋编号" width="120" sortable>
            </el-table-column>
            <!--            <el-table-column prop="uiid" v-if="false">-->
            <!--            </el-table-column>-->

            <el-table-column align="center" prop="area" label="房屋面积" width="120" sortable>
            </el-table-column>
            <el-table-column prop="price" label="价格" width="100" sortable>
                <!--                <el-table-column prop="password" label="密码" width="100" :formatter="formatSex" sortable>-->
            </el-table-column>
            <el-table-column prop="pay" align="center" label="付费方式" width="120" sortable>
            </el-table-column>
            <el-table-column prop="unitType" label="户型" width="120" sortable>
            </el-table-column>
            <el-table-column prop="floor" label="楼层" min-width="120" sortable>
            </el-table-column>

            <el-table-column prop="community" label="小区名" min-width="180" sortable>
            </el-table-column>
            <el-table-column prop="pubPerson" label="发布人" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="time" label="发布时间" min-width="120" sortable>
            </el-table-column>

            <el-table-column prop="type" label="状态" :formatter="formatType" min-width="120" sortable>
            </el-table-column>

            <!--            <el-table-column prop="isRecommend" label="是否推荐" :formatter="formatRecommend" min-width="120" sortable>-->
<!--            <el-table-column prop="isRecommend" label="是否推荐" :formatter="formatRecommend" min-width="120" sortable>-->

<!--                <template slot-scope="scope">-->
<!--                    <el-tag-->
<!--                            type="success" v-if="scope.row.isRecommend == '1'"-->
<!--                            disable-transitions>推荐</el-tag>-->
<!--                </template>-->
<!--            </el-table-column>-->
            <el-table-column prop="houseDesc" label="房屋描述" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="region" label="所属区域" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="audit" label="审核状态" min-width="120">
                <template slot-scope="scope">
                    <el-tag
                            type="success" v-if="scope.row.audit == '1'"
                            disable-transitions>已审核</el-tag>
                    <el-tag
                            v-if="scope.row.audit == '0'"
                            disable-transitions>未审核</el-tag>
                    <el-tag
                            type="danger" v-if="scope.row.audit == '2'"
                            disable-transitions>驳回</el-tag>
                </template>
            </el-table-column>

            <el-table-column label="操作" width="150" fixed="right">
                <template scope="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">通过</el-button>
                    <el-button type="danger" size="small" @click="handleDel(scope.$index, scope.row)">驳回</el-button>
                </template>
            </el-table-column>
        </el-table>

        <!--工具条-->
        <el-col :span="24" class="toolbar">
            <el-button type="danger" @click="batchRemove" :disabled="this.sels.length===0">批量删除</el-button>
            <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="pageSize" :total="total" style="float:right;">
            </el-pagination>
        </el-col>


    </section>
</template>

<script>
    import util from '../../common/js/util'
    // import axios from 'axios';
    //import NProgress from 'nprogress'
    import { getAuditListPage, removeAudit, batchRemoveUser, editAudit, addUser } from '../../api/api';

    export default {
        data() {
            return {
                filters: {
                    area: '',
                    price: '',
                    pay: '',
                    unitType: '',
                    floor: '',
                    community: '',
                    pubPerson: '',
                    time: '',
                    type: '',
                    isRecommend: '',
                    houseDesc: '',
                    region: '',
                    state: '',
                    audit: '',
                    region: '',
                    city: ''
                },
                audit: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 15,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    name: [
                        { required: true, message: '请输入姓名', trigger: 'blur' }
                    ]
                },
                //编辑界面数据
                editForm: {
                    // id: 0,
                    uid:0,
                    uiid:0,
                    username:'',
                    password: '',
                    name:'',
                    sex: '',
                    phone: '',
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
            formatType: function(row,column) {
                if(row.type == 1) {
                    return "租房";
                } else if (row.type == 2) {
                    return "新房";
                } else {
                    return "二手房";
                }
            },
            formatRecommend: function(row,column) {
                if(row.isRecommend == 1) {
                    return "推荐";
                } else {
                    return "";
                }
            },
            handleCurrentChange(val) {
                this.page = val;
                this.getAudit();
            },
            //获取用户列表
            getAudit() {
                var para = {
                    page: this.page,
                    area: this.filters.area,
                    price: this.filters.price,
                    pay: this.filters.pay,
                    unitType: this.filters.unitType,
                    floor: this.filters.floor,
                    community: this.filters.community,
                    type: this.filters.type,
                    region: this.filters.region,
                    city: this.filters.city,
                    pageSize: this.pageSize
                };
                this.listLoading = true;
                //NProgress.start();
                getAuditListPage(para).then((res) => {
                    // console.log(para)
                    // console.log(res.data.length)
                    console.log(res)
                    this.total = res.data.total;
                    this.audit = res.data.hps;
                    this.listLoading = false;
                    //NProgress.done();
                });
            },
            //删除
            handleDel: function (index, row) {
                // console.log(index,row)
                this.$confirm('确认退回该记录吗?', '提示', {
                    type: 'warning'
                }).then(() => {
                    this.listLoading = true;
                    //NProgress.start();
                    var para = { hid: row.hid };
                    removeAudit(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '退回成功',
                            type: 'success'
                        });
                        this.getAudit();
                    });
                }).catch(() => {

                });
            },
            //显示编辑界面
            handleEdit: function (index, row) {
                // console.log(index,row)
                this.$confirm('确认通过该记录吗?', '提示', {
                    type: 'warning'
                }).then(() => {
                    this.listLoading = true;
                    //NProgress.start();
                    var para = { hid: row.hid };
                    editAudit(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '通过成功',
                            type: 'success'
                        });
                        this.getAudit();
                    });
                }).catch(() => {

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
                        this.getAudit();
                    });
                }).catch(() => {

                });
            }
        },
        mounted() {
            this.getAudit();
        }
    }

</script>

<style scoped>

</style>