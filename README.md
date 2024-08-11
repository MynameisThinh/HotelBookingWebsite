Đây là project booking phòng ở khách sạn sử dụng blazor web SSR dùng identity trong .NET 8.
Em chỉ upload code thẳng lên github mà không tạo git từ đầu ở project và pull code dần dần lên là vì em chỉ làm 1 mình nên em không chú ý kỹ đến phần này vì vậy em xin lỗi nếu ai có phàn nàn về phần này ạ.
Đầu tiên phần xây dựng project thì em đã có phân tích nghiệp vụ và thiết kế phần mềm (đã có làm từ trước lúc năm 3) và thiết kế CSDL.
Sau đó sử dụng code first từ tạo các entities tương ứng với csdl,thêm vào dbcontext các bảng join với nhau cho đến thêm migration..
Phần UI thì em chỉ tham khảo trên các trang website sử dụng code html/css/js có sẵn và em chỉ chỉnh sửa tương đối vài phần trong đó.
Tiếp tục em tạo các service tương ứng với từng nghiệp vụ của bảng để xử lý backend sau đó phần code bên blazor sẽ gọi tùy vào blazor đó xử lý các nghiệp vụ nào mà dùng service tương ứng.
Để hoàn thiện project này em cũng tham khảo vài nguồn từ youtube, udemy cũng như chatGPT và stackoverflow. 
Sau đó cũng debug và fix lỗi (phần js làm em hơi mất thời gian khá nhiều vì nhiều lúc nó không hiện lỗi mà do thiếu inject vài thư viện trong blazor) test lại các chức năng kỹ lưỡng. 
Xây dựng xong trong vòng gần 4 tháng.(Nếu quá chậm thì cho em xin lỗi ạ)
Cảm ơn anh/chị đã quan tâm và đọc hết nội dung trên. Em xin cảm ơn.
link UI : https://htmlcodex.com/hotel-html-template/
