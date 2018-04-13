# CacheManager
应用缓存管理工具

CacheManager可以帮助管理应用中使用的外部缓存项目（目前仅支持Redis），提供缓存的查看、修改、清除等操作。

系统以项目为维度，使用时，需要首先创建项目App，在App下管理你的Key缓存项即可。


### Key键规则
Key值有两种创建规则，通过名称识别
1. 单项Key值，直接对应缓存服务器中特定的Key，如：`sys:cache:maxTTL`等
2. 多项Key值，一对多的映射，此Key代表一个Key值序列，该序列支持两种模式
    * 数值类型，自动+1或-1操作，如 `sys:cache:news:{1-10000}`
    * 字符类型, 仅支持Char单字符形式，如 `sys:cache:cluster{A-Z}`

两种模式和混合使用，如 `sys:cache:cluster{A-Z}:news{1-1000}`,则查询的Key键列表为:
```
sys:cache:clusterA:news1
system.cache:clusterA:news2
...
system.cache:clusterA:news1000
system.cache:clusterB:news1
...
system.cache:clusterZ:news1000
```

*注：多项Key值模式，适合针对单表Id自增等有数值或字符规则的批量缓存进行管理*

### Install And Run
* 创建cachemanager数据库，使用document中的install.sql创建表，可选择插入示例数据
* 安装.Net Core运行时环境
* 安装Redis本地环境或修改示例数据App的连接字符串到现有的缓存系统中
* 发布代码到站点目录，运行`dotnet run`命令可以查询是否成功

### Dependencies
* .net core runtime >= v1.1.2
* vue.js 2.x
* jQuery v2.2.0
* bootstrap v3.3.7
* bootstrap-treeview-1.2.0



### Changelog
Detailed changes for each release are documented in the [release notes](https://github.com/Skysper/CacheManager/releases).

### Contribution
Your contributing are welcomed.

### Document
Set more details here. 
[CacheManager Tool Instruction](http://skysper.me/2018/03/cache-manager-tool)
