<template>
	<section>

		<el-row>
			<el-col :span="8">
				<el-card class="box-card" shadow="hover">
					<div slot="header" class="clearfix">
						<span>用户统计</span>
					</div>
					<el-row>
						<el-col :span="8" style="text-align: center">
							今日新增用户
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ userToday }}</h3></span>
							</div>
						</el-col>
						<el-col :span="8" style="text-align: center">
							当月新增用户
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ userMonth }}</h3></span>
							</div>
						</el-col>
						<el-col :span="8" style="text-align: center">
							历史用户
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ userHis }}</h3></span>
							</div>
						</el-col>
					</el-row>
				</el-card>
			</el-col>

			<el-col :span="8">
				<el-card class="box-card" shadow="hover">
					<div slot="header" class="clearfix">
						<span>订单统计</span>
					</div>
					<el-row>
						<el-col :span="6" style="text-align: center">
							今日订单量
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ orderToday }}</h3></span>
							</div>
						</el-col>
						<el-col :span="6" style="text-align: center">
							当月待支付
							<br>
							<div style="padding-top: 2px">
								<span style="color: green;"><h3>{{ waitOrder }}</h3></span>
							</div>
						</el-col>
						<el-col :span="6" style="text-align: center">
							当月已支付
							<br>
							<div style="padding-top: 2px">
								<span style="color: green;"><h3>{{ placedOrder }}</h3></span>
							</div>
						</el-col>
						<el-col :span="6" style="text-align: center">
							历史订单量
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ orderHis }}</h3></span>
							</div>
						</el-col>
					</el-row>
				</el-card>
			</el-col>


			<el-col :span="8">
				<el-card class="box-card" shadow="hover">
					<div slot="header" class="clearfix">
						<span>房屋统计</span>
					</div>
					<el-row>
						<el-col :span="8" style="text-align: center">
							今日新增房屋数
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ houseToday }}</h3></span>
							</div>
						</el-col>
						<el-col :span="8" style="text-align: center">
							当月新增房屋数
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ houseMonth }}</h3></span>
							</div>
						</el-col>
						<el-col :span="8" style="text-align: center">
							历史房屋数
							<br>
							<div style="padding-top: 2px">
								<span style="color: red;"><h3>{{ houseHis }}</h3></span>
							</div>
						</el-col>
					</el-row>
				</el-card>
			</el-col>

			<el-col :span="12">
				<el-card class="box-card" shadow="hover">
					<div id="chartColumn" style="width:100%; height:400px;"></div>
				</el-card>
			</el-col>
			<el-col :span="12">

				<el-card class="box-card" shadow="hover">
<!--					<el-select v-model="provinces" placeholder="请选择" style="float: right" @change="changeProvince($event,scope.row)">-->
<!--						<el-option-->
<!--								v-for="item in provinces"-->
<!--								:label="item.provinces"-->
<!--								:value="item.provinces">-->
<!--						</el-option>-->
<!--					</el-select>-->
					<el-form>
						<el-form-item>
							<span style="font-size: 16px;font-weight: bold">房屋地区分布</span>
							<el-select v-model="form.pro" style="float: right" placeholder="河南省" @change="changeProvince">
								<el-option v-for="(item,index) in provinces" :value="item" :key="index" :label="item"></el-option>
							</el-select>
						</el-form-item>
					</el-form>

					<div id="chartBar" style="width:100%; height:340px;"></div>
				</el-card>

			</el-col>
		</el-row>

	</section>
</template>

<script>

	import echarts from 'echarts'
	import { getData, getProvince } from '../api/api'

	export default {
		data() {
			return {
				form:{
					pro: ''
				},
				temp: '',
				provinces:[],
				chartColumn: null,
				chartBar: null,
				chartLine: null,
				chartPie: null,
				userToday: 0,
				userMonth: 0,
				userHis: 0,
				orderToday: 0,
				orderMonth: 0,
				orderHis: 0,
				waitOrder: 0,
				placedOrder: 0,
				houseToday: 0,
				houseMonth: 0,
				houseHis: 0,
				userChartX: [],
				userChartY: [],
				houseDicX: [],
				houseDicY: [],
				housePie: [],
			}
		},
		methods: {
			changeProvince(val) {
				console.log(val)
				let obj = {};
				obj = this.provinces.find((item) => {
					return item === val;
				});
				let province = ''
				province = obj;
				this.temp = province;

				var par = {
					"province" : province
				}

				console.log('select city ->',province)

				this.getMainData(par)
			},
			drawColumnChart() {
				this.chartColumn = echarts.init(document.getElementById('chartColumn'));
				this.chartColumn.setOption({
					title: {
						text: '近七天用户房屋新增情况'
					},
					legend: {
						right: 8,
						data: ['新增用户数','新增房屋数']
					},
					tooltip: {
						trigger: 'axis',
						axisPointer: {
							type: 'cross',
							crossStyle: {
								color: '#999'
							}
						}
					},
					xAxis: {
						data: this.userChartX,
						axisPointer: {
							type: 'shadow'
						}
					},
					yAxis: [
						{
							type: 'value',
							name: '新增人数',
						},
						{
							type: 'value',
							name: '新增房屋数',
						},
					],
					series: [
						{
							label: {
								show: true,
							},
							name: '新增用户数',
							type: 'bar',
							data: this.userChartY
						},
						{
							name: '新增房屋数',
							type: 'line',
							yAxisIndex: 1,
							data: this.houseDicY
						}
					]
				});
			},
			drawBarChart() {
				this.chartBar = echarts.init(document.getElementById('chartBar'));
				this.chartBar.setOption({
					// title: {
					// 	text: '房屋地区分布',
					// 	left: 'left'
					// },
					tooltip: {
						trigger: 'item'
					},
					legend: {
						orient: 'vertical',
						left: 'left',
					},
					series: [
						{
							name: '房屋分布',
							type: 'pie',
							radius: '80%',
							data: this.housePie,
							emphasis: {
								itemStyle: {
									shadowBlur: 10,
									shadowOffsetX: 0,
									shadowColor: 'rgba(0, 0, 0, 0.5)'
								}
							}
						}
					]
				});
			},
			drawCharts() {
				this.drawColumnChart()
				this.drawBarChart()
				// this.drawLineChart()
				// this.drawPieChart()
			},
			getMainData(par){

				getData(par).then(res=>{
					console.log(res)
					this.userToday = res.data.userToday;
					this.userMonth = res.data.userMonth;
					this.userHis = res.data.userHis;
					this.orderToday = res.data.orderToday;
					this.orderMonth = res.data.orderMonth;
					this.orderHis = res.data.orderHis;
					this.waitOrder = res.data.waitOrder;
					this.placedOrder = res.data.placedOrder;
					this.houseToday = res.data.houseToday;
					this.houseMonth = res.data.houseMonth;
					this.houseHis = res.data.houseHis;
					this.userChartX = res.data.userChartX;
					this.userChartY = res.data.userChartY;
					// this.houseDicX = res.data.houseDicX;
					this.houseDicY = res.data.houseDicY;
					this.housePie = res.data.housePie

					this.drawCharts()

				});
			}
		},
		mounted() {
			console.log('main....')
			getProvince().then((data) => {
				this.provinces = data.data
			})
			let par = {
				'province': '河南省'
			}
			this.getMainData(par);
			console.log(this.housePie)
			// this.$router.options.routes[7].hidden = true
			// console.log(this.$store.state.auth)


			// this.getHouses();

		},
		// updated() {
		// 	console.log("update...")
		// 	this.getMainData();
		// 	this.drawCharts()
		//
		// }
	}

</script>

<style scoped>
	/*.self-Style {*/
	/*	background-color: #ffffff;*/
	/*}*/

	section {
		padding-top: 10px;
	}

	.text {
		font-size: 14px;
	}

	.item {
		margin-bottom: 18px;
	}

	.clearfix:before,
	.clearfix:after {
		display: table;
		content: "";
	}
	.clearfix:after {
		clear: both
	}

	/*.box-card {*/
	/*	width: 480px;*/
	/*	height: 200px;*/
	/*}*/

	.chart-container {
		width: 100%;
		float: left;
	}

	.el-row {
		margin-bottom: 20px;
	&:last-child {
		 margin-bottom: 0;
	 }
	}
	.el-col {
		padding: 30px 10px;
		font-size: 16px;
		border-radius: 4px;
	}
	.bg-purple-dark {
		background: #99a9bf;
	}
	.bg-purple {
		background: #d3dce6;
	}
	.bg-purple-light {
		background: #e5e9f2;
	}
	.grid-content {
		border-radius: 4px;
		min-height: 36px;
	}
	.row-bg {
		padding: 10px 0;
		background-color: #f9fafc;
	}

</style>