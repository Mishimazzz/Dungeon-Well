param (
    [string]$title = "通知",
    [string]$message = "内容"
)

# 调试输出确认是否运行
Write-Host "Toast script running..."
Write-Host "Title: $title"
Write-Host "Message: $message"

# 引入通知API
[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] > $null

# 创建通知模板
$template = @"
<toast>
  <visual>
    <binding template="ToastGeneric">
      <text>$title</text>
      <text>$message</text>
    </binding>
  </visual>
</toast>
"@

# 构建 Toast 对象
$xml = New-Object Windows.Data.Xml.Dom.XmlDocument
$xml.LoadXml($template)
$toast = [Windows.UI.Notifications.ToastNotification]::new($xml)
[Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier("UnityToast").Show($toast)
