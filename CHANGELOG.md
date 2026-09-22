# Changelog

Tất cả thay đổi đáng chú ý của VTC Core được ghi ở đây.
Định dạng theo [Keep a Changelog](https://keepachangelog.com/vi/1.1.0/),
đánh số theo [Semantic Versioning](https://semver.org/lang/vi/).

## [1.0.2] - 2026-09-22

### Fixed
- Khai báo `com.unity.ugui` trong `dependencies` của `package.json`.
  Project không có sẵn ugui (chỉ có module built-in `com.unity.modules.ui`) thì assembly
  `UnityEngine.UI` không tồn tại, nên tham chiếu thêm ở 1.0.1 vẫn không phân giải được và
  lỗi `Graphic could not be found` còn nguyên. Khác với git URL, package registry khai báo
  trong `dependencies` thì UPM tự cài được.

## [1.0.1] - 2026-09-22

### Fixed
- Thêm tham chiếu `UnityEngine.UI` vào 4 asmdef. Trước đó code dùng `Graphic`, `Image`,
  `Button`... mà chỉ khai báo `Unity.TextMeshPro`; trong project dev nó biên dịch được
  nhờ tham chiếu bắc cầu, nhưng cài sang project khác thì lỗi
  `CS0246: The type or namespace name 'Graphic' could not be found`.
- Gỡ `using UnityEditor;` thừa trong `NumberConverter.cs` — assembly runtime mà dùng
  namespace editor thì build player sẽ lỗi.
- Gỡ `using Unity.VisualScripting;` thừa trong `EditorExtension.cs`; extension
  `IsNullOrEmpty(this string)` đã có sẵn trong `Vtcong.Extensions`.

### Changed
- README ghi rõ Odin Inspector và DOTween là phụ thuộc bắt buộc.

## [1.0.0] - 2026-09-21

### Added
- Phát hành đầu tiên dưới dạng package UPM độc lập.
