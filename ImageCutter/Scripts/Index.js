var index = {
    imgSrcPath: '',
    cImage: undefined,
    init: function () {
        //1、初始化文件上传组件
        var uFile = new FileUpload();
        //console.log(uFile);
        uFile.init({
            element: '#input_uploadFile',
            uploadUrl: '/ImageCutter/Home/UploadFile?folder=head/src',
            uploadSuccess: function (data) {
                //console.log(data.filepath);
                //console.log(data.folder);
                index.imgSrcPath = data.filepath;
                $('#img-headSrc').attr('src', data.filepath);
                $('#src-container img').attr('src', data.filepath);
                $('#img-headPreview').attr('src', data.filepath);

                index.cImage = new ImageCrop();
                index.cImage.init({
                    prevContainer: '#preview-container',
                    imgSrc: '#img-headSrc'
                });
            }
        });

        $('#btn_uploadFile').click(function () {
            $("#modal_UploadFile").modal();
        });

        //2、初始化图片裁剪组件
        //var cImage = new ImageCrop();
        //cImage.init({
        //    prevContainer: '#preview-container',
        //    imgSrc: '#img-headSrc'
        //});

        //3、上传裁剪参数
        $('#btn_uploadHead').click(function () {
            //console.log(index.cImage.cutInfo);
            //console.log(index.imgSrcPath);
            if (!index.cImage.cutInfo.w || index.cImage.cutInfo.w == 0) {
                alert('请选择裁剪区域');
                return;
            }
            $.ajax({
                url: '/ImageCutter/Home/CutImage',
                type: 'post',
                async: false,
                data: {
                    x: index.cImage.cutInfo.x,
                    y: index.cImage.cutInfo.y,
                    w: index.cImage.cutInfo.w,
                    h: index.cImage.cutInfo.h,
                    srcClientWidth: $('#img-headSrc').width(),
                    srcClientHeight: $('#img-headSrc').height(),
                    imgSrcPath: index.imgSrcPath //'/ImageCutter/upload/head/src/5df3f764bb004bd5808b3c2d5b394eff.jpg'
                },
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    // data 可能是 xmlDoc, jsonObj, html, text, 等等...
                    //console.log(data);
                    alert(data.msg);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    // 通常 textStatus 和 errorThrown 之中
                    // 只有一个会包含信息
                    alert('error:' + textStatus);
                }
            });
        });
    }
}