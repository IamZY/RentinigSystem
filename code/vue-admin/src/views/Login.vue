<template>
  <div class="login">
    <div class="loginDiv">
      <el-form :model="ruleForm2" :rules="rules2" ref="ruleForm2" label-position="left" label-width="0px" class="demo-ruleForm login-container">
        <h3 class="title">系统登录</h3>
        <el-form-item prop="account">
          <el-input type="text" v-model="ruleForm2.account" auto-complete="off" placeholder="账号"></el-input>
        </el-form-item>
        <el-form-item prop="checkPass">
          <el-input type="password" v-model="ruleForm2.checkPass" auto-complete="off" placeholder="密码"></el-input>
        </el-form-item>
        <el-form-item prop="verifycode">
          <!-- 注意：prop与input绑定的值一定要一致，否则验证规则中的value会报undefined，因为value即为绑定的input输入值 -->
          <el-input v-model="ruleForm2.verifycode" placeholder="请输入验证码" class="identifyinput"></el-input>
        </el-form-item>
        <el-form-item>
          <div class="identifybox">
            <div @click="refreshCode">
              <s-identify :identifyCode="identifyCode"></s-identify>
            </div>
            <el-button @click="refreshCode" type='text' class="textbtn">看不清，换一张</el-button>
          </div>
        </el-form-item>
        <el-checkbox v-model="checked" checked class="remember">记住密码</el-checkbox>
        <el-form-item style="width:100%;">
          <el-button type="primary" style="width:100%;" @click.native.prevent="handleSubmit2" :loading="logining">登录</el-button>
          <!--<el-button @click.native.prevent="handleReset2">重置</el-button>-->
        </el-form-item>
      </el-form>
    </div>

  </div>

</template>

<script>
  import { requestLogin } from '../api/api';
  import SIdentify from '../components/ValidateCode'
  // import {}
  //import NProgress from 'nprogress'
  export default {
    data() {

      // 验证码自定义验证规则
      const validateVerifycode = (rule, value, callback) => {
        if (value === '') {
          callback(new Error('请输入验证码'))
        } else if (value !== this.identifyCode) {
          console.log('validateVerifycode:', value)
          callback(new Error('验证码不正确!'))
        } else {
          callback()
        }
      }
      return {
        // note: {
        //   backgroundImage: "url(" + require("../assets/001.jpg") + ")",
        //   backgroundRepeat: "no-repeat",
        //   backgroundSize: "25px auto",
        //   marginTop: "5px",
        // },
        logining: false,
        ruleForm2: {
          account: 'admin',
          checkPass: '123456',
          verifycode: ''
        },
        identifyCodes: '1234567890',
        identifyCode: '',
        rules2: {
          account: [
            { required: true, message: '请输入账号', trigger: 'blur' },
            //{ validator: validaePass }
          ],
          checkPass: [
            { required: true, message: '请输入密码', trigger: 'blur' },
            //{ validator: validaePass2 }
          ],
          verifycode: [
            { required: true, message: '请输入验证码', trigger: 'blur' },
            //{ validator: validaePass2 }
          ]
        },
        checked: true
      };
    },
    components: {
      SIdentify,
      // app
    },
    mounted() {
      // 验证码初始化
      this.identifyCode = '';
      this.makeCode(this.identifyCodes, 4)
    },
    methods: {
      handleReset2() {
        this.$refs.ruleForm2.resetFields();
      },
      handleSubmit2(ev) {

        if (this.identifyCode != this.ruleForm2.verifycode) {
          this.$message({
            message: "验证码错误",
            type: 'error'
          });
          return false;
        }


        var _this = this;
        this.$refs.ruleForm2.validate((valid) => {
          if (valid) {
            //_this.$router.replace('/table');
            this.logining = true;
            //NProgress.start();
            var loginParams = { username: this.ruleForm2.account, password: this.ruleForm2.checkPass };

              requestLogin(loginParams).then(data => {
                this.logining = false;
                let { msg, code, user, userAuth } = data;
                if (code !== "200") {
                  // if (false) {
                  this.$message({
                    message: msg,
                    type: 'error'
                  });
                } else {
                  sessionStorage.setItem('user', JSON.stringify(user));

                  this.$store.commit("setAuth",userAuth)

                  if (userAuth.main == "0") {
                    this.$router.options.routes[2].hidden = true
                  }else {
                    this.$router.options.routes[2].hidden = false
                  }
                  if (userAuth.users == "0") {
                    this.$router.options.routes[3].hidden = true
                  }else {
                    this.$router.options.routes[3].hidden = false
                  }
                  if (userAuth.orders == "0") {
                    this.$router.options.routes[4].hidden = true
                  }else {
                    this.$router.options.routes[4].hidden = false
                  }
                  if (userAuth.house == "0") {
                    this.$router.options.routes[5].hidden = true
                  } else {
                    this.$router.options.routes[5].hidden = false
                  }
                  if (userAuth.others == "0") {
                    this.$router.options.routes[6].hidden = true
                    this.$router.options.routes[7].hidden = true
                    // this.$router.options.routes[8].hidden = true
                  }else {
                    this.$router.options.routes[6].hidden = false
                    this.$router.options.routes[7].hidden = false
                  }

                  // console.log(this.$router)
                  // app.a=0;


                  this.$router.push({ path: '/Main' });
                }
              });


          } else {
            console.log('error submit!!');
            return false;
          }
        });
      },


      // 生成随机数
      randomNum(min, max) {
        return Math.floor(Math.random() * (max - min) + min)
      },
      // 切换验证码
      refreshCode() {
        this.identifyCode = ''
        this.makeCode(this.identifyCodes, 4)
      },
      // 生成四位随机验证码
      makeCode(o, l) {
        for (let i = 0; i < l; i++) {
          this.identifyCode += this.identifyCodes[
                  this.randomNum(0, this.identifyCodes.length)
                  ]
        }
        console.log(this.identifyCode)
      }



    }
  }

</script>

<style lang="scss">

  /*.note{*/
  /*  background-image: url("../assets/001.jpg");*/
  /*}*/
  /*#app {*/
  /*  background-image: url("../assets/001.jpg");*/
  /*  !*background: no-repeat;*!*/
  /*  background-repeat: no-repeat;*/
  /*  background-size: 100% 100%;*/
  /*}*/

  /*.self-Style{*/
  /*  background-image: url("../assets/001.jpg");*/
  /*  !*background: no-repeat;*!*/
  /*  background-repeat: no-repeat;*/
  /*  background-size: 100% 100%;*/
  /*}*/
  .login {
    width: 100%;
    height: 100%;
    background-color: #1d8ce0;
    /*background-size: cover;*/
    background-position: center;
    position: relative;
    background-image: url("../assets/001.jpg");
    background-repeat: no-repeat;
    background-size: 100% 100%;
  }

  /*.parent self-Style{*/
  /*      background-image: url("../assets/001.jpg");*/
  /*      background-repeat: no-repeat;*/
  /*      background-size: 100% 100%;*/
  /*}*/

  /*.parent >>> self-Style {*/
  /*    background-image: url("../assets/001.jpg");*/
  /*    !*background: no-repeat;*!*/
  /*    background-repeat: no-repeat;*/
  /*    background-size: 100% 100%;*/
  /*}*/

  .identifybox{
    display: flex;
    justify-content: space-between;
    margin-top:7px;
  }
  .iconstyle{
    color:#409EFF;
  }
  /*body {*/
  /*  background: url("../assets/001.jpg") center !important;*/
  /*  background-size: 100% 100%;*/
  /*}*/
  .login-container {
    position: absolute;
    /*box-shadow: 0 0px 8px 0 rgba(0, 0, 0, 0.06), 0 1px 0px 0 rgba(0, 0, 0, 0.02);*/
    -webkit-border-radius: 5px;
    border-radius: 5px;
    -moz-border-radius: 5px;
    background-clip: padding-box;
    margin-left: 40%;
    margin-top: 10%;
    width: 350px;
    /*padding-top: 50px;*/
    padding: 35px 35px 15px 35px;
    background: #fff;
    border: 1px solid #eaeaea;
    box-shadow: 0 0 25px #cac6c6;
    .title {
      margin: 0px auto 40px auto;
      text-align: center;
      color: #505458;
    }
    .remember {
      margin: 0px 0px 35px 0px;
    }
  }
</style>