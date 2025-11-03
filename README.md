
# 🚀 Store Management API (Backend)


-----

## 🛠️ 1. Hướng dẫn Chạy Dự án (Local Development)


### Yêu cầu Tiên quyết

  * **MySQL & XAMPP:** Đã cài đặt và đảm bảo dịch vụ MySQL đang chạy (đã tạo database tương ứng với tên trong file cấu hình).
  * **.NET SDK:** Đã cài đặt .NET SDK (Phiên bản dự án đang sử dụng, ví dụ: .NET 8).
  * Visual studio 
### Các bước Khởi động

1.  **Clone Project:**
    ```bash
    git clone [Link Repository Backend]
    cd StoreManagement.API
    ```
2.  **Cấu hình Database:**
      * Kiểm tra file `appsettings.Development.json` để xác nhận chuỗi kết nối (`ConnectionStrings:DefaultConnection`) khớp với cấu hình MySQL cục bộ của bạn.
3.  **Áp dụng Migration:**
 + bắt buộc phải tạo database trống trong mysql (store_manager) thí sau khi chạy lệnh mới có thể tự động sinh ra các bảng dữ liệu
 
    ```bash
    dotnet ef database update(terminal) / Update-Database(trong Package Manager Console)
    ```
      * Lệnh này sẽ tạo/cập nhật cấu trúc bảng trong database MySQL của bạn.
4.  **Khởi chạy API:**
    ```bash
    dotnet run / nhấn nút run
    ```
      * API sẽ khởi động và thông báo URL endpoint.
      * **Base URL **http://localhost:5254`** (Frontend cần dùng URL này làm cơ sở cho mọi API call).

-----

## 🔗 2. Tài liệu & Liên kết quan trọng

| Thuộc tính | Liên kết | Ghi chú |
| :--- | :--- | :--- |
| **Tài liệu API chi tiết** | [Link API Docs](https://surl.li/dvmyiu) | Dùng để kiểm tra request/response schema chi tiết, DTO và các mã lỗi. |
| **Sơ đồ Database (Db Diagram)** | [Link Db Diagram](https://surl.lu/biytdi) | Giúp hiểu cấu trúc dữ liệu và mối quan hệ giữa các bảng (Entity). |
| **Tài liệu thiết kế dự án** | [Link Docs Project](https://surl.li/toggdr) |thiết kế các module chức năng. |

-----

## 📋 3. Endpoint API Cốt lõi (Minimal Guide)

Đây là bảng tổng hợp các API chính đã triển khai.

| Module | Endpoint | Method | Công dụng (Mô tả) | Yêu cầu Auth |
| :--- | :--- | :--- | :--- | :--- |
| **Auth** | `/api/auth/register` | `POST` | Tạo tài khoản người dùng mới. | Không |
| **Auth** | `/api/auth/login` | `POST` | Đăng nhập và nhận **Access Token**. | Không |
| **User** | `/api/users/profile` | `GET` | Lấy thông tin người dùng hiện tại (dùng token). | **Có** |
| **Category** | `/api/categories` | `POST` | Tạo danh mục sản phẩm mới. | **Có** (Thường là vai trò Admin/Staff) |
| **Category** | `/api/categories` | `GET` | Lấy danh sách tất cả danh mục. | Không |
| **Product** | `/api/products` | `POST` | Tạo một sản phẩm (Sách) mới. | **Có** (Thường là vai trò Admin/Staff) |
| **Product** | `/api/products` | `GET` | Lấy danh sách tất cả sản phẩm. | Không |

**Quy tắc Authorization:**

  * Các API có "Yêu cầu Auth" cần Header: **`Authorization: Bearer <Access Token>`**
  * Frontend cần lưu trữ và sử dụng `AccessToken` nhận được từ `/api/auth/login`.

-----

## 🛑 4. Cấu trúc Phản hồi và Xử lý Lỗi

API sử dụng cấu trúc phản hồi chuẩn (JSON) cho mọi request thành công và thất bại:
*Response thành công:*

| Trường | Kiểu | Mô tả |
| :--- | :--- | :--- |
| `success` | `boolean` | `true` nếu thành công, `false` nếu thất bại. |
| `message` | `string` | Thông báo chi tiết hoặc lỗi (dùng để hiển thị cho người dùng). |
| `data` | `object`/`array` | Dữ liệu chính được trả về. |
| `statusCode` | `number` | Mã HTTP (200, 201,...). |

*Response thất bại:*

| Trường | Kiểu | Mô tả |
| :--- | :--- | :--- |
| `success` | `boolean` | `true` nếu thành công, `false` nếu thất bại. |
| `message` | `string` | Thông báo chi tiết hoặc lỗi (dùng để hiển thị cho người dùng). |
| `statusCode` | `number` | Mã HTTP (400, 500,...). |

*Response thất bại (lỗi đầu vào):*

| Trường | Kiểu | Mô tả |
| :--- | :--- | :--- |
| `success` | `boolean` | `true` nếu thành công, `false` nếu thất bại. |
| `message` | `string` | Thông báo chi tiết hoặc lỗi (dùng để hiển thị cho người dùng). |
| `errors`   | `string[]`| thông báo ra các lỗi của các trường cụ thể
| `statusCode` | `number` | Mã HTTP (400, 500,...). |