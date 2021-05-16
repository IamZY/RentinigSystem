import Vue from 'vue'
import Vuex from 'vuex'
import * as actions from './actions'
import * as getters from './getters'

Vue.use(Vuex)

// 应用初始状态
const state = {
    count: 10,
    auth: {
        uaid: 0,
        main: '0',
        users: '0',
        orders: '0',
        house: '0',
        others: '0',
        uid: 0
    }
}

// 定义所需的 mutations
const mutations = {
    INCREMENT(state) {
        state.count++
    },
    DECREMENT(state) {
        state.count--
    },
    setAuth: function (state, auth) {
        state.auth.uaid = auth.uaid,
        state.auth.main = auth.main,
        state.auth.users = auth.users,
        state.auth.orders = auth.orders,
        state.auth.house = auth.house,
        state.auth.others = auth.others,
        state.auth.uid = auth.uid
    }
}

// 创建 store 实例
export default new Vuex.Store({
    actions,
    getters,
    state,
    mutations
})