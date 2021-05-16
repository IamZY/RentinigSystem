import Login from './views/Login.vue'
import NotFound from './views/404.vue'
import Home from './views/Home.vue'
import Main from './views/Main.vue'
import Table from './views/nav1/Table.vue'
import Form from './views/nav1/Form.vue'
import user from './views/nav1/user1.vue'
import Page4 from './views/nav2/Page4.vue'
import Page5 from './views/nav2/Page5.vue'
import Page6 from './views/nav3/Page6.vue'
import echarts from './views/charts/echarts.vue'
import User from './views/nav1/User'
import House from './views/nav1/House'
import Order from './views/nav1/Order'
import Message from './views/nav1/Message'
import Audit from './views/nav1/Audit'

import UserAuth from './views/nav1/UserAuth'
import DataDic from './views/nav1/DataDic'

let routes = [
    {
        path: '/login',
        component: Login,
        name: '',
        hidden: true
    },
    {
        path: '/404',
        component: NotFound,
        name: '',
        hidden: true
    },
    {
        path: '/',
        component: Home,
        name: '',
        iconCls: 'fa fa-address-card',
        leaf: true,//只有一个节点
        hidden: false,
        children: [
            { path: '/main', component: Main, name: '主页' },
        ]
    },
    {
        path: '/',
        component: Home,
        name: '用户信息管理',
        iconCls: 'fa fa-lg fa-user',//图标样式class
        hidden: false,
        children: [
            { path: '/user', component: User, name: '用户信息管理' },
            { path: '/userAuth', component: UserAuth, name: '用户权限管理' },
        ]
    },
    {
        path: '/',
        component: Home,
        name: '订单管理',
        iconCls: 'fa fa-lg fa-first-order',//图标样式class
        hidden: false,
        children: [
            // { path: '/main', component: Main, name: '主页', hidden: true },
            // { path: '/user', component: User, name: '用户信息管理' },
            // { path: '/house', component: House, name: '房屋信息管理' },
            { path: '/order', component: Order, name: '订单管理' },
            // { path: '/message', component: Message, name: '留言管理' },
            // { path: '/audit', component: Audit, name: '房屋审核' },
            // { path: '/table', component: Table, name: 'Table' },
            // { path: '/form', component: Form, name: 'Form' },
            // { path: '/user', component: user, name: '列表' },
        ]
    },
    {
        path: '/',
        component: Home,
        name: '房屋管理',
        iconCls: 'fa fa-lg fa-home',//图标样式class
        hidden: false,
        children: [
            // { path: '/main', component: Main, name: '主页', hidden: true },
            // { path: '/user', component: User, name: '用户信息管理' },
            { path: '/house', component: House, name: '房屋信息管理' },
            // { path: '/order', component: Order, name: '订单管理' },
            // { path: '/message', component: Message, name: '留言管理' },
            { path: '/audit', component: Audit, name: '房屋审核' },
            // { path: '/table', component: Table, name: 'Table' },
            // { path: '/form', component: Form, name: 'Form' },
            // { path: '/user', component: user, name: '列表' },
        ]
    },
    {
        path: '/',
        component: Home,
        name: '',
        iconCls: 'fa fa-usb',
        leaf: true,//只有一个节点
        hidden: false,
        children: [
            { path: '/message', component: Message, name: '留言管理' },
        ]
    },
    // {
    //     path: '/',
    //     component: Home,
    //     name: '导航二',
    //     iconCls: 'fa fa-id-card-o',
    //     children: [
    //         { path: '/page4', component: Page4, name: '页面4' },
    //         { path: '/page5', component: Page5, name: '页面5' }
    //     ]
    // },
    // {
    //     path: '/',
    //     component: Home,
    //     name: '',
    //     iconCls: 'fa fa-address-card',
    //     leaf: true,//只有一个节点
    //     children: [
    //         { path: '/page6', component: Page6, name: '导航三' }
    //     ]
    // },
    {
        path: '/',
        component: Home,
        name: '',
        iconCls: 'fa fa-bar-chart',
        leaf: true,//只有一个节点
        hidden: false,
        children: [
            { path: '/DataDic', component: DataDic, name: '数据字典' }
        ]
    },
    {
        path: '*',
        hidden: true,
        redirect: { path: '/404' }
    }
];

export default routes;