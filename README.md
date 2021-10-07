# [English](https://github.com/egametang/Egametang/blob/master/README-EN.md) 

__讨论QQ群 : 474643097__  

[ET论坛](https://et-framework.cn)  

# 本项目是ET6.0接入ILRuntime代码热更新，xAsset资源热更新，FairyGUI跨平台UI的示例，后续将保持维护状态，与ET同步更新。

接入ILRtuntime教程地址：[https://www.lfzxb.top/et-6-with-ilruntime/](https://www.lfzxb.top/et-6-with-ilruntime/) (完全仿照ET 5.0接入ILRuntime的思路进行开发的) 但已过时，出于学习的目的，可以前往：[et6以5.0思路接入ilrt里程碑](https://github.com/wqaetly/ET/releases/tag/ilrt-change) 进行查看

**当前master采用了一种与上述博客不同的，全新的思路**

- 热更层：当前master会把Model, ModelView，Hotfix，HotfixView打成一个程序集，然后以全热更的方式进行，最大限度保留了ET的结构，方便后续跟进更新
- 非热更层：Unity.Mono，整个框架的底层驱动，网络协议的收发与序列化都在这里

# 环境 && 版本

 - Unity：2020.3.16
 - 服务端：.Net 5.0
 - 客户端：.Net Framework 4.7.2
 - IDE：Rider 2020.2
 - ET：commit 1535
 - FGUI：2021.3.1
 - ILRuntime：commit 1231
 - xAsset 4.0

# ET的介绍：

ET是一个开源的游戏客户端（基于unity3d）服务端双端框架，服务端是使用C# .net core开发的分布式游戏服务端，其特点是开发效率高，性能强，双端共享逻辑代码，客户端服务端热更机制完善，同时支持可靠udp tcp websocket协议，支持服务端3D recast寻路等等

# 致命BUG

ILRuntime模式下，如果往一个async ETVoid/ETTask函数中传递Scene参数，并且这个函数最终产生了异常（ETTask.SetException）就会引发崩溃，目前原因仍然未知，请务必避免这种情况的出现！

参见这个Commit：[https://github.com/wqaetly/ET/commit/ee5483de2c4f8b9bf053f23ae62eaf504035a306](https://github.com/wqaetly/ET/commit/ee5483de2c4f8b9bf053f23ae62eaf504035a306)

因为测试用例过于特殊，如果传递除Scene以外的任何值，都不会由于ETTask.SetException崩溃，所以我连issue都不知道要怎么提，只能一直跟进ET和ILRuntime的更新，说不定哪天机缘一到这个致命BUG就好了

其实我是猜测因为ETTask把ILRT的运行堆栈给搞烂了，但是代码翻来覆去也没找到什么缘由，只好作罢（顺带一提，如果开启了ILRuntime的调试服务，即Appdomain.DebugService.StartDebugService(56000); 就不会崩溃了，会卡一下，然后程序继续进行）

# TODO && Features

- [x] 接入 [ILRuntime](https://github.com/Ourpalm/ILRuntime)
- [x] 接入 [xAsset](https://github.com/xasset/xasset) 作为资源管理方案（必要，因为当前非热更层直接从Resources目录加载的Hotfix Dll和配置数据，而这些数据理应是热更的）
- [x] 更新 [ET 6.0学习笔记](https://www.lfzxb.top/et6.0-study/)
- [x] 更新 [FGUI](https://www.fairygui.com/) 代码生成插件
- [x] 更新 [ET使用FGUI开发的工作流程](https://www.lfzxb.top/et-fguilearn/)
- [ ] 新增 [FGUI基于Lua的插件开发指南]
- [ ] 以ET 6.0为底层框架，更新 [状态帧同步Moba，包含基于双端行为树技能系统](https://gitee.com/NKG_admin/NKGMobaBasedOnET)

# 项目截图

![项目热更新演示](https://user-images.githubusercontent.com/35335061/130990459-4818145a-7ce3-4e39-95bc-0a2048e78c7a.png)

# 引用

[ET 6.0学习笔记](https://www.lfzxb.top/et6.0-study/)

[状态帧同步Moba，包含基于双端行为树技能系统](https://gitee.com/NKG_admin/NKGMobaBasedOnET)

[ET框架视频教程-6.0版本](https://space.bilibili.com/33595745/favlist?fid=759596845&ftype=create)

[91焦先生ET-ILRuntime](https://github.com/mister91jiao/ET_ILRuntime/)
