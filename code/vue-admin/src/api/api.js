import axios from 'axios';
import qs from 'qs';
let base = 'http://localhost:4313/Admin';
export const getUserListPage = params => { return axios.get(`${base}/getUser`, { params: params }); };
export const editUser = params => { return axios.post(`${base}/editUser`, params).then(res=>res.data); };
export const requestLogin = params => { return axios.post(`${base}/Login`, params).then(res => res.data); };
export const getUserList = params => { return axios.get(`${base}/user/list`, { params: params }); };
export const removeUser = params => { return axios.get(`${base}/deleteUser`, { params: params }); };
export const batchRemoveUser = params => { return axios.get(`${base}/user/batchremove`, { params: params }); };
export const addUser = params => { return axios.post(`${base}/addUser`,params).then(res=>res.data); };


export const getHouseListPage = params => { return axios.get(`${base}/getHouse`, { params: params }); };
export const recommendUser = params => { return axios.get(`${base}/recommendUser`, { params: params }); };
export const addHouse = params => { return axios.post(`${base}/addHouse`,params).then(res=>res.data); };
export const removeHouse = params => { return axios.get(`${base}/deleteHouse`, { params: params }); };
export const editHouse = params => {return axios.post(`${base}/editHouse`, params).then(res=>res.data)};


export const getOrdersListPage = params => { return axios.get(`${base}/getOrders`, { params: params }); };
export const removeOrder = params => { return axios.get(`${base}/deleteOrder`, { params: params }); };
export const editOrder = params => {return axios.post(`${base}/editOrder`, params).then(res=>res.data)};

export const getMessagesListPage = params => { return axios.get(`${base}/getMessage`, { params: params }); };
export const removeMessage = params => { return axios.get(`${base}/deleteMessage`, { params: params }); };
export const editMessage = params => {return axios.post(`${base}/editMessage`, params).then(res=>res.data)};

export const getAuditListPage = params => { return axios.get(`${base}/getAudit`, { params: params }); };
export const removeAudit = params => { return axios.get(`${base}/removeAudit`, { params: params }); };
export const editAudit = params => { return axios.get(`${base}/editAudit`, { params: params }); };



export const getArea = () => {return axios.get(`${base}/getRegion`);}


export const getData = params => {return axios.get(`${base}/getMainData`,{params:params});}


export const getUserAuthListPage = params => { return axios.get(`${base}/getUserAuth`, { params: params }); };

export const updateAuth = params => { return axios.get(`${base}/updateAuth`, { params: params }); };


export const getProvince = () => {return axios.get(`${base}/getProvince`);}
export const getCity = params => {return axios.get(`${base}/getCity`,{ params: params });}
export const getRegion = params => {return axios.get(`${base}/getRegion`,{ params: params });}


export const getDataDicListPage = params => { return axios.get(`${base}/getDataDic`, { params: params }); };
export const addDataDic = params => { return axios.post(`${base}/addDataDic`,params).then(res=>res.data); };
export const removeDataDic = params => { return axios.get(`${base}/removeDataDic`, { params: params }); };
export const editDataDic = params => { return axios.get(`${base}/editDataDic`, { params: params }); };