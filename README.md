# SnakeQuest

## 项目记录

### 项目特色？

- 游戏有画图/音频等
- 有页面切换
- 考虑类库？

### 项目大纲
1. 初始界面 图片，游戏名称，开始， 排名
1.1 跑马灯


2. 游戏窗体 
2.1 游戏开始，结束，游戏分数，计入排名
2.2 考虑一个菜单（包含 查看排名，游戏【暂停/重启/结束】，自定义【声音大小/贪吃蛇颜色】，游戏声明）

TODO:
- [ ] 游戏结束后，考虑弹出窗口（是否将成绩录入？然后一步步操作 -- 添加用户【并将成绩绑定】）

3. 排名（数据库设计）
3.1 昵称 | 时间 | 分数 | 累计次数 |
3.2 涉及的操作：排序（按照时间/分数/累计次数）

TODO:
- [ ] 排行榜：窗口弹出，布局样式，排序(?)

### 待补充部分
- [ ] 标题跑马灯，我想在WPF中将TextBlock和Timer绑定，但遇到一些困难，待解决
- [ ] `SizeToContent="WidthAndHeight"` 在窗体的属性使用，对于同一个窗体如何解决不同控件调换？


### 项目的布局
- 页面部分
    - 页面背后的代码直接用于交互控制页面内容
- 控制部分
    - command/
        - 控制🐍的移动
    - model/
        - 绘制画布/蛇/事物
    - Game.cs 游戏逻辑主体


## 参考资料
[wpf doc](https://learn.microsoft.com/zh-cn/dotnet/desktop/wpf/overview/?view=netdesktop-8.0)
[WPF 中的部分高级区域 doc](https://learn.microsoft.com/zh-cn/dotnet/desktop/wpf/advanced/?view=netframeworkdesktop-4.8)

books:
Pro WPF4.5 (4th)


### learn by practice!

#### C#部分

1. `int? x = null` ?符号表示Nullable类型，可以避免空引用

2. partial
> Partial classes allow you to split a
class into two or more separate pieces for development and fuse them together in the compiled assembly.
>> When you compile your application, the XAML that defines your user interface (such as Window1.
xaml) is translated into a CLR (common language runtime) type declaration that is merged with the logic
in your code-behind class file (such as Window1.xaml.cs) to form one single unit.

#### 命令交互

##### `RouterEventArgs`

![image-20240506171116861](https://yeijon-note.oss-cn-beijing.aliyuncs.com/img/image-20240506171116861.png)

WPF 应用程序通常包含许多元素，它们要么在 XAML 中声明，要么在代码中实例化。 应用程序的元素存在于其元素树中。 根据路由事件的定义方式，当事件在源元素上被引发时，它会： 

-  通过元素树从源元素浮升到根元素（通常是页面或窗口）。

- 通过元素树从根元素到源元素向下进行隧道操作。

- 不会遍历元素树，只发生在源元素上。

  

  来看看下面的一部分元素树：

  ```xaml
  <Border Height="30" Width="200" BorderBrush="Gray" BorderThickness="1">
      <StackPanel Background="LightBlue" Orientation="Horizontal" Button.Click="YesNoCancelButton_Click">
          <Button Name="YesButton">Yes</Button>
          <Button Name="NoButton">No</Button>
          <Button Name="CancelButton">Cancel</Button>
      </StackPanel>
  </Border>
  ```

  这三个按钮中的每一个都是潜在的 [Click](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.controls.primitives.buttonbase.click#system-windows-controls-primitives-buttonbase-click) 事件源。 单击其中一个按钮时，它会引发 `Click` 事件，从按钮浮升到根元素。 [Button](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.controls.button) 和 [Border](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.controls.border) 元素没有附加事件处理程序，但 [StackPanel](https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.controls.stackpanel) 有。 树中较高、未显示的其他元素可能也附加了 `Click` 事件处理程序。 当 `Click` 事件到达 `StackPanel` 元素时，WPF 事件系统将调用附加到它的 `YesNoCancelButton_Click` 处理程序。 示例中 `Click` 事件的事件路由为：`Button`\>`StackPanel`\>`Border`\> 连续的父元素。

```c#
private void YesNoCancelButton_Click(object sender, RoutedEventArgs e)
{
    FrameworkElement sourceFrameworkElement = e.Source as FrameworkElement;
    switch (sourceFrameworkElement.Name)
    {
        case "YesButton":
            // YesButton logic.
            break;
        case "NoButton":
            // NoButton logic.
            break;
        case "CancelButton":
            // CancelButton logic.
            break;
    }
    e.Handled = true;
}
```

关于如何创建一个 `RouteEventArgs`详见书籍



###### WPF Events

![image-20240506172449340](https://yeijon-note.oss-cn-beijing.aliyuncs.com/img/image-20240506172449340.png)









##### 容器变换
1. 在主窗口的XAML文件中定义两个UserControl的容器，并使用`Visibility`属性来控制它们的显示与隐藏：
```xml
<Grid>
    <local:UserControlA x:Name="userControlA" Visibility="Visible"/>
    <local:UserControlB x:Name="userControlB" Visibility="Collapsed"/>
</Grid>
```

2. 在页面A的按钮点击事件中，设置页面A的Visibility为Collapsed，同时设置页面B的Visibility为Visible：
```csharp
private void Button_Click(object sender, RoutedEventArgs e)
{
    userControlA.Visibility = Visibility.Collapsed;
    userControlB.Visibility = Visibility.Visible;
}
```
---



#### 控件

| 面板控件      | 区别和特点                                                                                               | 适用情况                                                                                                |
|--------------|---------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------|
| Canvas       | - 自由定位子元素，使用绝对坐标布局。<br>- 子元素位置可以通过 Canvas.Left 和 Canvas.Top 属性指定。         | - 需要精确控制子元素位置的场景。<br>- 绘图、图形等需要绝对定位的元素。                                |
| WrapPanel    | - 沿着水平或垂直方向自动换行排列子元素。<br>- 子元素会自动换行到下一行或列以避免溢出。                    | - 需要实现流畅排列、根据空间自动换行排列子元素的场景。                                                |
| StackPanel   | - 沿着单个方向（水平或垂直）顺序排列子元素。<br>- 子元素沿着指定方向依次放置，不会换行。                  | - 垂直或水平堆叠显示一系列控件。<br>- 显示菜单项、按钮组等线性布局的场景。                            |
| DockPanel    | - 将子元素停靠在面板的边缘或中心位置。<br>- 可以指定每个子元素相对于面板四个边缘之一进行停靠布局。        | - 希望将子控件停靠在特定位置，如顶部、底部、左侧、右侧等固定位置的布局场景。                          |
| Grid         | - 使用网格布局来定义行和列，并将子元素放置在网格单元中。<br>- 可以灵活定义多行多列，并设置各个单元格大小。   | - 复杂布局需求，需要将界面划分为不规则网格进行布局。<br>- 表格型数据展示等需要灵活网格布局的场景。     |

---


#### 组织关系

**用户控件（UserControl）**：
用户控件是一种自定义可重复使用的 UI 元素，类似于自定义控件或部件。
用户控件通常封装了一组相关功能或布局，并可以在多个窗口、页面中重复使用。
用户控件可包含多个子元素和逻辑代码，并提供更高级别的封装性。

窗口是最顶层的 UI 容器，在其内部可以包含其他 UI 元素，代表整个应用程序的主界面。一个 WPF 应用程序至少包含一个主窗口。
页面通常用于实现多页面导航结构，在 Frame 控件中加载和显示。每个页面代表一个独立内容单元，类似于网页中不同页面之间的切换。
用户控件为自定义 UI 元素，封装了特定功能或布局，并可嵌入到窗口、页面中重复使用。

---

#### 样式

要实现在 WPF 中调用的 UserControl 适配到主 Window 窗体的大小，可以通过以下方法进行适配：

1. 使用布局容器：将 UserControl 放置在适当的布局容器中（如 Grid、StackPanel、DockPanel 等），并设置布局容器的尺寸和对齐方式来控制 UserControl 的大小和位置。
2. 设置 UserControl 大小：可以在 UserControl 的 XAML 中设置 Width 和 Height 属性，或者使用 HorizontalAlignment 和 VerticalAlignment 属性来指定控件在父容器中的水平和垂直对齐方式。
3. 动态调整大小：可以通过绑定或代码动态调整 UserControl 的大小，以适应窗体大小变化。可以监听窗体的 SizeChanged 事件，在事件处理程序中根据窗体尺寸变化调整 UserControl 的大小。
4. 使用 ViewBox 控件：ViewBox 控件是一个自动缩放其内容以填充可用空间的控件，可以包裹 UserControl，并随着父容器大小自动缩放和适配。

示例：
```xml
<Grid>
    <Viewbox Stretch="Uniform">
        <local:YourUserControl/>
    </Viewbox>
</Grid>
```

在上述示例中，将 YourUserControl 放置在 ViewBox 中，并设置 Stretch 属性为 Uniform，这样 YourUserControl 将会自动缩放以适应父容器的空间。这种方法可以帮助您实现用户控件与主窗体之间的大小适配。

---

哭了，调整了很久的image source的资源定位方式，结果还是直接无脑操作简单...
