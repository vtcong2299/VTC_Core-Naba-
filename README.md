# VTC Core

Bộ tiện ích dùng chung: EventManager, FSM, Process, Extensions, UI, WeightedRandomizer,
UnbiasedTime, GuideMask, pool nội bộ và một số công cụ Editor.

## Cài đặt

Unity ▸ **Window ▸ Package Manager ▸ + ▸ Add package from git URL**:

```
https://github.com/vtcong2299/VTC_Core-Naba-.git
```

> **Phụ thuộc [VTC Singleton](https://github.com/vtcong2299/VTC_Singleton)** (phần UI dùng
> `Singleton<T>`) — **package tự cài giúp bạn**, không cần làm gì thêm.

Ngoài ra cần **TextMeshPro** (`com.unity.textmeshpro`), Unity thường cài sẵn.

## Assembly

| Assembly | Nội dung |
|---|---|
| `vtcong.core.runtime` | EventManager, FSM, Process, Extensions, Utils, WeightedRandomizer |
| `vtcong.core.editor` | Công cụ Editor: AnimatorHashGenerator, toolbar extender, tiện ích |
| `vtcong.ui.runtime` | UIManager, Quick Engine |
| `vtcong.ui.editor` | Trình vẽ inspector cho UI |

## Dependency được cài tự động thế nào

Unity Package Manager **không** phân giải được git URL khai báo trong trường
`dependencies` của `package.json` — chỉ `manifest.json` của project mới nhận git URL.

Nên package này kèm một assembly Editor nhỏ (`*.bootstrap`) chạy lúc editor khởi động:
nó kiểm tra kiểu `Vtcong.Core.Singleton<T>` có tồn tại không, nếu không thì gọi
`Client.Add(gitUrl)` để cài VTC Singleton. Assembly đó cố ý không tham chiếu tới
assembly nào của package, để khi thiếu Singleton nó vẫn biên dịch được và chạy để sửa.

Kiểm tra theo kiểu chứ không theo tên package, nên nếu bạn đã có VTC_Singleton nằm
thẳng trong `Assets/` thì nó sẽ không cài trùng.

Muốn khai báo tay thì thêm vào `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.vtcong.singleton": "https://github.com/vtcong2299/VTC_Singleton.git",
    "com.vtcong.core": "https://github.com/vtcong2299/VTC_Core-Naba-.git"
  }
}
```
