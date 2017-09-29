# CacheManager 
应用缓存管理工具

CacheManager可以帮助管理应用中使用的外部缓存项目（当前仅支持Redis），提供缓存的查看、修改（暂不支持）、清除缓存等操作。系统以项目为维度，使用时，需要首先创建项目App，在App下管理你的Key缓存项即可。


### Key创建规则
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
cache:clusterZ:news1000
```

*注：多项Key值模式，适合针对单表Id自增等有数值或字符规则的批量缓存进行管理*

### Install
* 创建cachemanager数据库，使用document中的install.sql创建表，可选择插入示例数据

* 安装.net core
* 安装Redis本地环境或修改示例数据App的连接字符串到现有的缓存系统中


