//const { OidcClient } = require("oidc-client");
//记录log信息到pre标签的方法
function log(){
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function(msg){
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string'){
            msg  = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}
//为3个按钮添加点击事件
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("logout").addEventListener("click", logout, false);
document.getElementById("api").addEventListener("click", api, false);

var config = {
    authority: "https://localhost:50012",
    client_id: "js",
    redirect_uri: "https://localhost:5003/callback.html",
    response_type: "code",
    scope: "openid profile api1",
    post_logout_uri: "https://localhost:5003/index.html",
};
//实例化 UserManager，oidc-client类库里面的usermanager类用来管理openid连接协议
//UserManager提供一个getUser api来获取user是否登入js应用程序,返回的user对象的profile属性包含user的claims
var mgr = new Oidc.UserManager(config);
mgr.getUser().then(function(user){
    if (user) {
        log("User logged in", user.profile);
    }
    else{
        log("User not logged in");
    }
});
//实现登入，登出，和api函数
function login(){
    mgr.signinRedirect();
}
function api() {
    mgr.getUser().then(function (user) {
        var url = "https://localhost:60011/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET",url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}













