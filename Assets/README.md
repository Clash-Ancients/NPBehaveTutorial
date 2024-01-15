# NPBehaveTutorial
learn how to use NPBehave, read source code

## 资源地址：
- 1 npbehave github:https://github.com/meniku/NPBehave
- 2 本工程 github：https://github.com/Clash-Ancients/NPBehaveTutorial

# 规则
```c#
规则 1 : 只能启动非激活节点

规则 2 ： 装饰节点始终只有一个字节点 && 大多数情况下，装饰节点下面跟随的是组合节点

规则 3 ： 节点之间的启动时Start(), 节点父类调用节点多态子类用DoStart()
```

## 节点介绍

### Node
```c++
- 1 基类节点
- 2 virtual void DoStart()
- 3 virtual void Start()
- 4 virtual SetRoot(Root _root)
- 5 virtual SetParent(Container parent)

//变量
- 1 string m_name
- 2 Root m_Root
- 3 Node m_parentNode
```

### Container
```c++
 - 1 继承自Node
 - 2 ChildStopped, DoChildStopped
```

### Decorator
```c++
- 1 继承自Container
- 2 override SetRoot(Root _rootNode)
```

### Root
```c++


//变量
 - 1 Node m_mainNode
```

### Service
```c++
- 1 继承自Decorator
- 2 添加 serviceMethod
- 3 添加 m_interval
- 4 添加 m_randomVariation
```

# Tur_1 : 通过root, service打印一条日志
```c++
- 1 路径： Assets/TurScenes/Tur_1
- 2 通过这个例子，可以理解npbehave的基础框架逻辑
```