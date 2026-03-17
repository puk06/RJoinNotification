# RJoinNotification

VRChatワールド用の入退室通知システムです。

このギミックを利用するには[Canvas Animation System](https://github.com/puk06/CanvasAnimationSystem)を予め導入する必要があります。

## Settings

### Has Background

不透明なバックグラウンドを追加します。

### Mode

- Fade In
- Pop
- Fade In From Left
- Fade In From Right
- Fade In From Below

ただいまPopが大変重くなっております。

下のAllow Multiple Notificationsは、必ずオフにしてください。

### Allow Multiple Notifications

同時に複数人が入退室した場合では、複数（Notificationオブジェクトの数）の通知が重ねて表示されます。

バックグラウンドがない場合では、文字が重ねて表示されるため、読みにくくなる場合があります。

### Notifications

表示される通知のオブジェクトです。順番に影響がないが、同じオブジェクトを複数スロットに入れないでください。

### Join Sound / Exit Sound

入室・退室時のオーディオです。

### Canvas Aninmation System (Max Concurrent Animations)

同時実行できるアニメーションの数です。

Pop以外のおすすめ: 64 Popのおすすめ: 128

ほかの設定は確認用のため、変更してもなにも効果がありません。

### R Join Notification Object

Notification Objectの設定は、オブジェクトをまとめて変更するか、プレハブで変更してください。

#### Join Text / Exit Text

入室・退室時に表示される、入室か退室かを表示する文字です。

#### Transition In Time / Transition Out Time

アニメーションが出現、消失する時間の長さです。

#### Stay Time

出現アニメーションが終わってから、消失アニメーションが始まるまでの長さです。

#### Pop Mode Offset

Pop Modeを利用する場合で、右へのオフセットです。デフォルト:120。

#### Join / Exit Info Color

入室・退室のときに通知の色です。

### isMuted オブジェクト

RJoinNotificationの下にあるisMutedオブジェクトは、入退室通知の音が流れるかどうかを制御するオブジェクトです。このオブジェクトは外部のスイッチなどによる操作が可能です。

例えば、Lura's SwitchのTargetObjectに入れると、Lura's Switchでの制御が可能です。

> WIP