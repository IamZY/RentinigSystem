<template>
<!--	<div id="app" :class="[selfStyle]">-->
	<div id="app">
		<transition name="fade"
		            mode="out-in">
			<router-view></router-view>
		</transition>
	</div>
</template>

<script>
export default {
	name: 'app',
	data(){
		return {
			selfStyle: 'self-Style',
		}
	},
	created() {
		// 在页面加载时读取sessionStorage里的状态信息
			if (sessionStorage.getItem('store')) {
				this.$store.replaceState(
						Object.assign(
								{},
								this.$store.state,
								JSON.parse(sessionStorage.getItem('store'))
						)
			)

			if (this.$store.state.auth.main == "0") {
				this.$router.options.routes[2].hidden = true
			}else {
				this.$router.options.routes[2].hidden = false
			}
			if (this.$store.state.auth.users == "0") {
				this.$router.options.routes[3].hidden = true
			}else {
				this.$router.options.routes[3].hidden = false
			}
			if (this.$store.state.auth.orders == "0") {
				this.$router.options.routes[4].hidden = true
			}else {
				this.$router.options.routes[4].hidden = false
			}
			if (this.$store.state.auth.house == "0") {
				this.$router.options.routes[5].hidden = true
			}else {
				this.$router.options.routes[5].hidden = false
			}
			if (this.$store.state.auth.others == "0") {
				this.$router.options.routes[6].hidden = true
				this.$router.options.routes[7].hidden = true
				// this.$router.options.routes[8].hidden = true
			} else {
				this.$router.options.routes[6].hidden = false
				this.$router.options.routes[7].hidden = false
				// this.$router.options.routes[8].hidden = true
			}

		}

		// 在页面刷新时将vuex里的信息保存到sessionStorage里
		// beforeunload事件在页面刷新时先触发
		window.addEventListener('beforeunload', () => {
			sessionStorage.setItem('store', JSON.stringify(this.$store.state))
		})
	},
	components: {
	}
}

</script>

<style lang="scss">
	/*.self-Style{*/
	/*	background-image: url("assets/001.jpg");*/
	/*	!*background: no-repeat;*!*/
	/*	background-repeat: no-repeat;*/
	/*	background-size: 100% 100%;*/
	/*}*/
body {
	margin: 0px;
	padding: 0px;
	/*background: url(assets/bg1.jpg) center !important;
		background-size: cover;*/
	// background: #1F2D3D;
	font-family: Helvetica Neue, Helvetica, PingFang SC, Hiragino Sans GB, Microsoft YaHei, SimSun, sans-serif;
	font-size: 14px;
	-webkit-font-smoothing: antialiased;
}

#app {
	position: absolute;
	top: 0px;
	bottom: 0px;
	left: 0px;
	right: 0px;
	width: 100%;
	height: 100%;
}

.el-submenu [class^=fa] {
	vertical-align: baseline;
	margin-right: 10px;
}

.el-menu-item [class^=fa] {
	vertical-align: baseline;
	margin-right: 10px;
}

.toolbar {
	background: #f2f2f2;
	padding: 10px;
	//border:1px solid #dfe6ec;
	margin: 10px 0px;
	.el-form-item {
		margin-bottom: 10px;
	}
}

.fade-enter-active,
.fade-leave-active {
	transition: all .2s ease;
}

.fade-enter,
.fade-leave-active {
	opacity: 0;
}
</style>