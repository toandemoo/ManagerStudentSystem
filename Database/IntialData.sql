USE TaekwondoDB;
GO

INSERT INTO Taekwondo.Levels (LevelName) VALUES
('Cấp 10'),
('Cấp 9'),
('Cấp 8'),
('Cấp 7'),
('Cấp 6'),
('Cấp 5'),
('Cấp 4'),
('Cấp 3'),
('Cấp 2'),
('Cấp 1'),
('1 Đẳng'),
('2 Đẳng'),
('3 Đẳng'),
('4 Đẳng'),
('5 Đẳng'),
('6 Đẳng'),
('7 Đẳng'),
('8 Đẳng'),
('9 Đẳng'),
('10 Đẳng');

-- INSERT INTO Taekwondo.InvoiceStatus (StatusName, Description) VALUES
-- ('Draft', 'Hóa đơn mới tạo, chưa phát hành'),
-- ('Issued', 'Đã phát hành, đang chờ thanh toán'),
-- ('PartiallyPaid', 'Đã thanh toán một phần'),
-- ('Paid', 'Đã thanh toán đủ'),
-- ('Overdue', 'Quá hạn nhưng chưa thanh toán đủ'),
-- ('Cancelled', 'Hủy hóa đơn'),
-- ('Refunded', 'Đã hoàn tiền');

-- INSERT INTO Taekwondo.PaymentStatus (StatusName, Description) VALUES
-- ('Pending', 'Đang chờ thanh toán'),
-- ('Processing', 'Giao dịch đang được xử lý'),
-- ('Succeeded', 'Thanh toán thành công'),
-- ('Failed', 'Thanh toán thất bại'),
-- ('Cancelled', 'Giao dịch bị hủy'),
-- ('Expired', 'Giao dịch đã hết hạn'),
-- ('Refunded', 'Đã hoàn tiền'),
-- ('PartiallyRefunded', 'Đã hoàn tiền một phần');

-- INSERT INTO Taekwondo.PaymentAttemptStatus (StatusName, Description) VALUES
-- ('Created', 'Payment attempt vừa được tạo'),
-- ('Initiated', 'Đã gửi yêu cầu đến payment provider'),
-- ('Pending', 'Đang chờ kết quả thanh toán'),
-- ('Succeeded', 'Payment provider xác nhận thanh toán thành công'),
-- ('Failed', 'Thanh toán thất bại'),
-- ('Cancelled', 'Giao dịch bị hủy'),
-- ('Expired', 'Payment attempt đã hết thời gian thanh toán');


-- INSERT INTO Taekwondo.PaymentTransactionStatus (StatusName, Description) VALUES
-- ('Pending', 'Transaction đang chờ xử lý'),
-- ('Authorized', 'Payment provider đã authorize giao dịch'),
-- ('Captured', 'Đã capture tiền từ payment provider'),
-- ('Succeeded', 'Transaction hoàn tất thành công'),
-- ('Failed', 'Transaction thất bại'),
-- ('Cancelled', 'Transaction bị hủy'),
-- ('Expired', 'Transaction đã hết hạn'),
-- ('Refunded', 'Transaction đã được hoàn tiền toàn bộ'),
-- ('PartiallyRefunded', 'Transaction đã được hoàn tiền một phần');

INSERT INTO Taekwondo.PaymentMethods (PaymentMethodName) VALUES
('Tiền mặt'),
('Chuyển khoản ngân hàng'),
('Ví điện tử'),
('Thẻ tín dụng/ghi nợ'),
('Thanh toán trực tuyến');

INSERT INTO Taekwondo.FeePlans (FeePlanName, FeeAmountMonthly, Amount, DisCountPercentage, Description) VALUES
('Gói tháng', 1, 300, 0,'học phí theo tháng'),
('Gói khóa học', 3, 3600, 0, 'học phí theo khóa học 3 tháng với miễn phí võ phục'),
('Gói năm', 12, 3600, 0, 'học phí theo năm với miễn phí võ phục và lệ phí thi lần đầu');

INSERT INTO Taekwondo.Roles (RoleName, Description) VALUES
('Admin', 'Quản trị viên toàn hệ thống'),
('Manager', 'Quản lý câu lạc bộ'),
('Coach', 'Huấn luyện viên'),
('Staff', 'Nhân viên câu lạc bộ'),
('Member', 'Võ sinh thành viên'),
('Parent', 'Phụ huynh của võ sinh');


INSERT INTO Taekwondo.PaymentProviders (PaymentProviderCode, PaymentProviderName) VALUES
('VNPAY', 'VNPAY'),
('MOMO', 'MOMO'),
('ZALOPAY', 'ZALOPAY'),
('PAYPAL', 'PAYPAL'),
('STRIPE', 'STRIPE');

INSERT INTO Taekwondo.ItemTypes (ItemTypeName, Description) VALUES
('Võ phục', 'Võ phục Taekwondo'),
('Bảo hiểm', 'Bảo hiểm cho võ sinh'),
('Lệ phí thi', 'Lệ phí thi lên cấp, lên đẳng');

INSERT INTO Taekwondo.Items (ItemName, ItemTypeId, Price) VALUES
('Võ phục Taekwondo', 1, 500000 ),
('Bảo hiểm võ sinh', 2, 200000 ),
('Lệ phí thi lên cấp', 3, 300000 );