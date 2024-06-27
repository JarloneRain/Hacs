# Hacs

Hacs是一个使用使用鼠标实现快捷键操作的工具。

Hacs is a tool that allows for keyboard shortcut operations using the mouse.

Hacs的名字是“Hexagon-alt-ctrl-shift”的简写，分别对应以六边形为主的UI和三个修饰键。

The name "Hacs" is an abbreviation for "Hexagon-alt-ctrl-shift," corresponding to the UI primarily based on hexagons and three modifier keys.

## 使用 Usage

使用Hacs发送一个快捷键的流程如下（以QQ截图快捷键Ctrl+Alt+A为例）：

1. 启动Hacs；
2. 在你需要截图的界面按下鼠标右键并向下拖动，此时屏幕上会出现一个底边与x轴平行、顶角朝上的蓝绿色的半透明整三角形，然后松开鼠标右键；
3. 松开鼠标右键，此时屏幕上会出现若干个蓝绿色半透明六边形，在其中找到带有镂空文字为“A”的六边形，用鼠标左键单击即可完成快捷键操作。

The process of sending a shortcut key using Hacs (taking the QQ screenshot shortcut Ctrl+Alt+A as an example) is as follows:

1. Launch Hacs;
2. On the interface where you need to take a screenshot, press and drag the right mouse button downwards. At this time, a blue-green semi-transparent isosceles triangle with its base parallel to the x-axis and its apex pointing upwards will appear on the screen, then release the right mouse button;
3. After releasing the right mouse button, several blue-green semi-transparent hexagons will appear on the screen. Find the hexagon with the hollowed-out letter "A" and click it with the left mouse button to complete the shortcut key operation.

## 功能 Fuctions

### 开机自启动 Auto-start on boot

还未实现。

Not implement.

### 常驻托盘 Resides in the system tray

在启动Hacs后能在右下角的系统托盘看到一个Hacs的Logo。单击这个图标就能打开Home窗口，右键这个图标能打开一个菜单。

After launching Hacs, you can see a Hacs logo in the system tray at the bottom right corner. Clicking this icon will open the Home window, and right-clicking it will open a menu.

### 家窗口 Home

现在还只是一个窗口

Only a window.

### 日志窗口 Logs

通过系统托盘图标的右键菜单进入。

Accessed through the right-click menu of the system tray icon.

### 多显示器支持 Multi-monitor support

几乎没做。

Almost not implemented.

## 饼 TODO 

* 解决出现KeysForm后会导致鼠标卡顿的问题
* 实现发送快捷键的功能
* 实现Home界面，用户可以在这里配置快捷键
* 在托盘图标的菜单中添加一个开关

别画了，吃不下了……

* Fixed the issue where the mouse would lag after the KeysForm appears
* Implemented the function to send shortcut keys
* Implemented the Home interface where users can configure shortcut keys
* Add a switch to the menu of the tray icon

Wow~! Absolutely fantastic work!