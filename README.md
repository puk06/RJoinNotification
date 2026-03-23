# RJoinNotification

VRChatワールド用の入退室通知システムです。

このギミックを利用するには[Canvas Animation System](https://github.com/puk06/CanvasAnimationSystem)を予め導入する必要があります。

導入するには、VPMをご利用ください。

https://vpm.rurinya.com/vpm.json

## Settings

### Join Text / Exit Text

入室・退室時に表示される通知の内容です。

### Join / Exit Info Color

入室・退室の通知の色です。

### Transition In Time / Transition Out Time

アニメーションが出現、消失するアニメーションの長さです。

### Stay Time

出現アニメーションが終わってから、消失アニメーションが始まるまでの長さです。

### Has Background

不透明なバックグラウンドが追加されます。

### Audio Only

音声のみ流されます。

### Mode

- Fade In
- Pop
- Fade In From Left
- Fade In From Right
- Fade In From Below

ただいまPopが大変重くなっております。

下のAllow Multiple Notificationsは、必ずオフにしてください。

### Pop Mode Offset

Pop Modeを利用する場合で、右へのオフセットです。デフォルト:120。

### Allow Multiple Notifications

同時に複数人が入退室した場合では、複数（Notificationオブジェクトの数）の通知が重ねて表示されます。

バックグラウンドがない場合では、文字が重ねて表示されるため、読みにくくなる場合があります。

### Join Sound / Exit Sound

入室・退室音です。

### Notifications

表示される通知のオブジェクトです。多いほどAllow Multiple Notificationsで表示できる通知の数が多くなりますが、重くなります。順番に影響がないが、同じオブジェクトを複数スロットに入れないでください。

### Canvas Aninmation System (Max Concurrent Animations)

RJoinNotificationにあるCanvasオブジェクトにアタッチされています。

同時実行できるアニメーションの数です。

Pop以外のおすすめ: 64 Popのおすすめ: 128

Pop以外はNotificationオブジェクトの数\*12+10, PopではNotificationオブジェクトの数\*22+10にしてください。

ほかの設定は確認用のため、変更しても効果がありません。

### R Join Notification Object

Notification Objectの設定は、すべてレファレンスになっております。

そのため、通常使用では編集する必要がありません。


### isMuted オブジェクト

RJoinNotificationの下にあるisMutedオブジェクトは、入退室通知の音が流れるかどうかを制御するオブジェクトです。このオブジェクトは外部のスイッチなどによる操作が可能です。

例えば、Lura's SwitchのTargetObjectに入れると、Lura's Switchでの制御が可能です。

> WIP