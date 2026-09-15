# Hướng Dẫn Sử Dụng Ứng Dụng Chat (Mạng LAN / Wi-Fi)

Tài liệu này hướng dẫn cách kết nối hai máy tính khác nhau để sử dụng ứng dụng Chat.
* **Máy A:** Đóng vai trò là Máy chủ (Server) và có thể mở thêm 1 Client để chat.
* **Máy B:** Đóng vai trò là Máy khách (Client).

---

## ⚠️ Cấu Hình Bắt Buộc Trước Khi Bắt Đầu
Do ứng dụng được thiết kế chạy với đặc quyền quản trị hệ thống (Privilege / Run as Administrator) để đảm bảo kết nối mạng không bị giới hạn, bạn cần lưu ý:
1. **Chấp nhận quyền Admin:** Khi mở ứng dụng, nếu Windows hiện bảng hỏi `User Account Control (UAC)`, bắt buộc phải chọn **Yes**.
2. **Sửa lỗi gõ Tiếng Việt:** Nếu không gõ được tiếng Việt trên ứng dụng, bạn phải tắt hẳn Unikey/EVKey hiện tại, sau đó mở lại bộ gõ này bằng cách click chuột phải chọn **Run as administrator**.

---

## Phần 1: Thao Tác Trên Máy A (Máy Chủ)

### Bước 1: Lấy địa chỉ IP mạng nội bộ
1. Nhấn phím `Windows + R`, gõ `cmd` và nhấn **Enter**.
2. Trong cửa sổ đen, gõ lệnh `ipconfig` và nhấn **Enter**.
3. Tìm đến dòng **IPv4 Address** (ví dụ: `192.168.1.15`). Hãy ghi lại dãy số này.

### Bước 2: Cho phép Server đi qua Tường lửa (Firewall)
1. Mở menu Start, tìm và chọn **Windows Defender Firewall with Advanced Security**.
2. Ở cột bên trái, chọn **Inbound Rules**.
3. Ở cột bên phải, chọn **New Rule...**
4. Chọn **Port** -> *Next* -> Nhập cổng đang dùng (ví dụ: `5050`) vào ô *Specific local ports* -> *Next*.
5. Chọn **Allow the connection** -> *Next* -> *Next* -> Đặt tên cho rule (VD: `ChatApp_Server`) -> *Finish*.

### Bước 3: Cập nhật IP và xuất file cho Máy B
1. Mở mã nguồn của ứng dụng Client (WPF) trên Máy A.
2. Tìm đến dòng kết nối TCP và thay đổi địa chỉ IP thành IP vừa lấy ở Bước 1:
   ```csharp
   _client = new TcpClient("192.168.1.15", 5050); // Thay IP và Port tương ứng
