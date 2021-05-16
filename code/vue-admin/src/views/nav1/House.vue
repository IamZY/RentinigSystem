<template>
    <section>
        <!--工具条-->
        <el-col :span="24" class="toolbar" style="padding-bottom: 0px;">
            <el-form :inline="true" :model="filters">
                <el-form-item>
                    <el-input v-model="filters.area" @change="getHouses" placeholder="房屋面积"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.price" @change="getHouses" placeholder="价格"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.pay" @change="getHouses" placeholder="付费方式"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-input v-model="filters.unitType" @change="getHouses" placeholder="户型"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.floor" @change="getHouses" placeholder="楼层"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.community" @change="getHouses" placeholder="小区名"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.region" @change="getHouses" placeholder="区县"></el-input>
                </el-form-item>

                <el-form-item>
                    <el-input v-model="filters.city" @change="getHouses" placeholder="城市"></el-input>
                </el-form-item>

                <el-form-item>
                    <!--                    <el-input v-model="filters.type" @change="getHouses" placeholder="小区名"></el-input>-->
                    <el-select v-model="filters.type" placeholder="租房类型" @change="getHouses">
                        <el-option value="" label="全部"></el-option>
                        <el-option value="1" label="租房"></el-option>
                        <el-option value="2" label="新房"></el-option>
                        <el-option value="3" label="二手房"></el-option>
                    </el-select>
                </el-form-item>

                <el-form-item>
                    <el-button type="primary" v-on:click="getHouses">查询</el-button>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="handleAdd">新增</el-button>
                </el-form-item>
            </el-form>
        </el-col>

        <!--列表-->
        <el-table :data="houses" highlight-current-row v-loading="listLoading" @selection-change="selsChange"
                  style="width: 100%;">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column type="index" width="60" v-if="false">
            </el-table-column>

            <el-table-column prop="hid" align="center" label="房屋编号" width="120" sortable>
            </el-table-column>


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
            <el-table-column prop="region" label="区县" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="city" label="城市" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="province" label="省" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="pubPerson" label="发布人" min-width="120" sortable>
            </el-table-column>
            <el-table-column prop="time" label="发布时间" min-width="120" sortable>
            </el-table-column>

            <el-table-column prop="type" label="房屋类型" :formatter="formatType" min-width="120" sortable>
            </el-table-column>

            <el-table-column prop="state" label="房屋状态" min-width="120" sortable>
                <template slot-scope="scope">
                    <el-tag
                            type="danger" v-if="scope.row.state == '0'"
                            disable-transitions>未租
                    </el-tag>

                    <el-tag
                            type="success" v-if="scope.row.state == '1'"
                            disable-transitions>已租
                    </el-tag>
                </template>
            </el-table-column>

            <!--            <el-table-column prop="isRecommend" label="是否推荐" :formatter="formatRecommend" min-width="120" sortable>-->
            <el-table-column prop="isRecommend" label="是否推荐" :formatter="formatRecommend" min-width="120" sortable>

                <template slot-scope="scope">
                    <el-tag
                            type="success" v-if="scope.row.isRecommend == '1'"
                            disable-transitions>推荐
                    </el-tag>
                </template>
            </el-table-column>
            <el-table-column prop="houseDesc" label="房屋描述" min-width="120" sortable>
            </el-table-column>

            <el-table-column label="操作" width="250" fixed="right">
                <template scope="scope">
                    <el-button size="small" v-if="scope.row.isRecommend != '1'"
                               @click="handleEdit(scope.$index, scope.row)">推荐
                    </el-button>
                    <el-button size="small" v-else @click="handleEdit(scope.$index, scope.row)">取消推荐</el-button>
                    <el-button type="primary" size="small" @click="editHouses(scope.$index, scope.row)">编辑</el-button>
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

                <el-form-item prop="hid" v-model="editForm.hid" v-if="false"></el-form-item>

                <el-form-item label="省" prop="province">
                    <el-select prop="province" v-model="editForm.province" @change="changeCity">
                        <el-option v-for="(item,index) in province" :label="item" :key="index"
                                   :value="item"></el-option>
                    </el-select>

                </el-form-item>

                <el-form-item label="城市" prop="city">
                    <el-select prop="city" v-model="editForm.city" @change="changeRegion">
                        <el-option v-for="(item,index) in city" :label="item" :key="index" :value="item"></el-option>
                    </el-select>
                </el-form-item>


                <el-form-item label="区县" prop="region">
                    <el-select prop="region" v-model="editForm.region">
                        <el-option v-for="(item,index) in region" :label="item" :key="index" :value="item"></el-option>
                    </el-select>
                </el-form-item>


                <el-form-item label="小区名" prop="community">
                    <el-input v-model="editForm.community" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="价格" prop="price">
                    <el-input v-model="editForm.price" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="付费方式" prop="pay">
                    <el-input v-model="editForm.pay" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="户型" prop="unitType">
                    <el-input v-model="editForm.unitType" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="楼层" prop="floor">
                    <el-input v-model="editForm.floor" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="类型" prop="type">
                    <el-select v-model="editForm.type" prop="type">
                        <el-option value="1" label="租房"></el-option>
                        <el-option value="2" label="新房"></el-option>
                        <el-option value="3" label="二手房"></el-option>
                    </el-select>
                    <!--                    <el-input v-model="addForm.type" auto-complete="off"></el-input>-->
                </el-form-item>
                <el-form-item label="描述" prop="houseDesc">
                    <el-input v-model="editForm.houseDesc" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="面积" prop="area">
                    <el-input v-model="editForm.area" auto-complete="off"></el-input>
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

                <el-form-item label="省" prop="province">
                    <el-select prop="province" v-model="addForm.province" @change="changeCity">
                        <el-option v-for="(item,index) in province" :label="item" :key="index"
                                   :value="item"></el-option>
                    </el-select>

                </el-form-item>

                <el-form-item label="城市" prop="city">
                    <el-select prop="city" v-model="addForm.city" @change="changeRegion">
                        <el-option v-for="(item,index) in city" :label="item" :key="index" :value="item"></el-option>
                    </el-select>
                </el-form-item>


                <el-form-item label="区县" prop="region">
                    <el-select prop="region" v-model="addForm.region">
                        <el-option v-for="(item,index) in region" :label="item" :key="index" :value="item"></el-option>
                    </el-select>
                </el-form-item>


                <el-form-item label="小区名" prop="community">
                    <el-input v-model="addForm.community" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="价格" prop="price">
                    <el-input v-model="addForm.price" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="付费方式" prop="pay">
                    <el-input v-model="addForm.pay" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="户型" prop="unitType">
                    <el-input v-model="addForm.unitType" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="楼层" prop="floor">
                    <el-input v-model="addForm.floor" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="类型" prop="type">
                    <el-select v-model="addForm.type" prop="type">
                        <el-option value="1" label="租房"></el-option>
                        <el-option value="2" label="新房"></el-option>
                        <el-option value="3" label="二手房"></el-option>
                    </el-select>
                    <!--                    <el-input v-model="addForm.type" auto-complete="off"></el-input>-->
                </el-form-item>
                <el-form-item label="描述" prop="houseDesc">
                    <el-input v-model="addForm.houseDesc" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="面积" prop="area">
                    <el-input v-model="addForm.area" auto-complete="off"></el-input>
                </el-form-item>
                <el-form-item label="图片" prop="pic">
                    <el-upload
                            ref="upload"
                            multiple
                            name="File"
                            action=""
                            :auto-upload="false"
                            :http-request="httpRequest"
                    >
                        <el-button size="small" type="primary">点击上传</el-button>
                        <span slot="tip" class="el-upload__tip">&nbsp;&nbsp;&nbsp;只能上传jpg/png文件</span>
                    </el-upload>
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
    import {
        getHouseListPage,
        removeHouse,
        editHouse,
        recommendUser,
        addHouse,
        getProvince,
        getCity,
        getRegion
    } from '../../api/api';

    export default {
        data() {
            return {
                value: true,
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
                username: [],
                fileList: [],
                region: [],
                users: [],
                total: 0,
                page: 1,
                listLoading: false,
                sels: [],//列表选中列
                pageSize: 15,

                editFormVisible: false,//编辑界面是否显示
                editLoading: false,
                editFormRules: {
                    community: [
                        {required: true, message: '请输入小区名'}
                    ],
                    price: [
                        {required: true, message: '请输入租金'},
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
                    pay: [
                        {required: true, message: '请输入付费方式，月'}
                    ],
                    unitType: [
                        {required: true, message: '请输入户型'}
                    ],
                    floor: [
                        {required: true, message: '请输入楼层'}
                    ],
                    houseDesc: [
                        {required: true, message: '请输入房屋描述'}
                    ],
                    area: [
                        {required: true, message: '请输入面积' },
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
                    ]
                },
                //编辑界面数据
                editForm: {
                    hid: '',
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
                    province: '',
                    city: ''
                },

                addFormVisible: false,//新增界面是否显示
                addLoading: false,
                addFormRules: {
                    community: [
                        {required: true, message: '请输入小区名', trigger: 'blur'}
                    ],
                    price: [
                        {required: true, message: '请输入租金'},
                        {
                            validator: function (rule, value, callback) {
                                let reg = /^[1-9]([0-9])*$/
                                let flag = reg.test(value);
                                if (!flag) {
                                    callback(new Error('必须整数'))
                                } else {
                                    callback()
                                }
                            }, trigger: 'blur'
                        }
                    ],
                    pay: [
                        {required: true, message: '请输入付费方式，月'}
                    ],
                    unitType: [
                        {required: true, message: '请输入户型' }
                    ],
                    floor: [
                        {required: true, message: '请输入楼层' }
                    ],
                    houseDesc: [
                        {required: true, message: '请输入房屋描述' }
                    ],
                    area: [
                        {required: true, message: '请输入面积' },
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
                    ]
                },
                //新增界面数据
                addForm: {
                    province: '',
                    city: '',
                    hid: '',
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
                    audit: ''
                },
                province: [],
                city: []

            }
        },
        methods: {
            // validateNum(rule,value,callback) {
            //     let reg = /^[1-9]([0-9])*$/
            //     let flag = reg.test(value);
            //     if (!flag) {
            //         callback(new Error('必须整数'))
            //     } else {
            //         callback()
            //     }
            // },
            changeCity(val) {
                //locations是v-for里面的也是datas里面的值
                let obj = {};
                obj = this.province.find((item) => {
                    return item === val;
                });
                let province = ''
                province = obj;
                // console.log(getName)

                var pars = {
                    "province": province
                }
                // this.region = []
                // this.editForm.city = ""
                // this.addForm.city = ""
                getCity(pars).then((data) => {
                    // console.log(data)
                    // this.editForm.city = ""
                    // this.addForm.city = ""
                    this.city = data.data
                })

            },

            changeRegion(val) {
                //locations是v-for里面的也是datas里面的值
                let obj = {};
                obj = this.city.find((item) => {
                    return item === val;
                });
                let city = ''
                city = obj;
                console.log(city)

                var pars = {
                    "city": city
                }
                // this.editForm.region = ""
                // this.addForm.region = ""
                getRegion(pars).then((data) => {
                    console.log(data)
                    this.region = data.data
                })
            },

            httpRequest(param) {
                console.log(param)
                this.fileList.push(param.file)
            },

            handleRemove(file, fileList) {
                console.log(file, fileList);
            },
            handlePreview(file) {
                console.log(file);
            },
            handleExceed(files, fileList) {
                this.$message.warning(`当前限制选择 3 个文件，本次选择了 ${files.length} 个文件，共选择了 ${files.length + fileList.length} 个文件`);
            },
            beforeRemove(file, fileList) {
                return this.$confirm(`确定移除 ${file.name}？`);
            },
            beforeUpload(file) {
                console.log(file)
                return false
            },


            formatType: function (row, column) {
                if (row.type == 1) {
                    return "租房";
                } else if (row.type == 2) {
                    return "新房";
                } else {
                    return "二手房";
                }
            },
            formatRecommend: function (row, column) {
                if (row.isRecommend == 1) {
                    return "推荐";
                } else {
                    return "";
                }
            },
            handleCurrentChange(val) {
                this.page = val;
                this.getHouses();
            },
            //获取用户列表
            getHouses() {
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
                getHouseListPage(para).then((res) => {
                    // console.log(para)
                    // console.log(res.data.length)
                    console.log(res)
                    this.total = res.data.total;
                    this.houses = res.data.hps;
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
                    var para = {hid: row.hid};
                    removeHouse(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.getHouses();
                    });
                }).catch(() => {

                });
            },
            editHouses: function (index, row) {
                this.editFormVisible = true;
                this.editForm = Object.assign({}, row);
            },
            //显示编辑界面
            handleEdit: function (index, row) {
                // console.log(index,row)
                this.$confirm('确认修改该推荐吗?', '提示', {
                    type: 'warning'
                }).then(() => {
                    this.listLoading = true;
                    //NProgress.start();
                    var para = {hid: row.hid};
                    recommendUser(para).then((res) => {
                        this.listLoading = false;
                        //NProgress.done();
                        this.$message({
                            message: '修改成功',
                            type: 'success'
                        });
                        this.getHouses();
                    });
                }).catch(() => {

                });
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

                            var upData = new FormData();

                            console.log(this.editForm)

                            upData.append('hid', this.editForm.hid);
                            upData.append('region', this.editForm.region);
                            upData.append('price', this.editForm.price);
                            upData.append('pay', this.editForm.pay);
                            upData.append('unitType', this.editForm.unitType);
                            upData.append('floor', this.editForm.floor);
                            upData.append('type', this.editForm.type);
                            upData.append('houseDesc', this.editForm.houseDesc);
                            upData.append('area', this.editForm.area);
                            upData.append('community', this.editForm.community);
                            upData.append('province', this.editForm.province);
                            upData.append('city', this.editForm.city);
                            // para.birth = (!para.birth || para.birth == '') ? '' : util.formatDate.format(new Date(para.birth), 'yyyy-MM-dd');
                            editHouse(upData).then((res) => {
                                this.editLoading = false;

                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['editForm'].resetFields();
                                this.editFormVisible = false;
                                this.getHouses();
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
                            this.$refs.upload.submit();
                            var upData = new FormData();
                            this.fileList.forEach(function (file) {// 因为要上传多个文件，所以需要遍历
                                upData.append('images', file, file.name);
                                // upData.append('file', this.file); //不要直接使用我们的文件数组进行上传，你会发现传给后台的是两个Object
                            })
                            var user = sessionStorage.getItem('user');

                            if (user) {
                                user = JSON.parse(user);
                                this.username = user.username || '';
                            }

                            upData.append('user', this.username);
                            upData.append('region', this.addForm.region);
                            upData.append('price', this.addForm.price);
                            upData.append('pay', this.addForm.pay);
                            upData.append('unitType', this.addForm.unitType);
                            upData.append('floor', this.addForm.floor);
                            upData.append('type', this.addForm.type);
                            upData.append('houseDesc', this.addForm.houseDesc);
                            upData.append('area', this.addForm.area);
                            upData.append('community', this.addForm.community);
                            upData.append('province', this.addForm.province);
                            upData.append('city', this.addForm.city);
                            addHouse(upData).then((res) => {
                                this.addLoading = false;
                                //NProgress.done();
                                this.$message({
                                    message: '提交成功',
                                    type: 'success'
                                });
                                this.$refs['addForm'].resetFields();
                                this.addFormVisible = false;
                                this.getHouses();
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
            this.region = sessionStorage.getItem('region')
            // console.log(this.region)
            getProvince().then((data) => {
                this.province = data.data
            })
            this.getHouses();
        }
    }

</script>

<style scoped>

</style>