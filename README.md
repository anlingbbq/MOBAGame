# MOBAGame
游戏素材来自LOL，服务器使用PhotonServer


## 简单的写一些日志：

### -17/3/29 
　这两天折腾服务器的缓存层，由于nhibernate中对数据对象的外键列表，即一对多的关系中，操作时会进行关联查询，单纯的当作对象使用，容易出现类似no session or session was closed的错误，每次都在session中处理又感频繁，这里我使用一个缓存层将用户的角色列表进行深拷贝保存起来。不知道这种处理好不好。。话说花这么多事件搞服务器真的好吗。
