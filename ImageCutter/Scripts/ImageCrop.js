var ImageCrop = function () {
    var crop = new Object();
    crop.cutInfo = {};
    crop.init = function (options) {
        var prevContainer = $(options.prevContainer),
        imgPreview = prevContainer.find('img'),
        xsize = prevContainer.width(),
        ysize = prevContainer.height();

        //console.log('init', [xsize, ysize]);

        var jcrop_api;
        $(options.imgSrc).Jcrop({
            onChange: updatePreview,
            onSelect: updatePreview,
            aspectRatio: xsize / ysize
        }, function () {
            // Use the API to get the real image size
            var bounds = this.getBounds();
            boundx = bounds[0];
            boundy = bounds[1];
            // Store the API in the jcrop_api variable
            jcrop_api = this;
        });

        function updatePreview(c) {
            if (parseInt(c.w) > 0) {
                //记录裁剪参数
                crop.cutInfo = c;

                var rx = xsize / c.w;
                var ry = ysize / c.h;

                imgPreview.css({
                    width: Math.round(rx * boundx) + 'px',
                    height: Math.round(ry * boundy) + 'px',
                    marginLeft: '-' + Math.round(rx * c.x) + 'px',
                    marginTop: '-' + Math.round(ry * c.y) + 'px'
                });
            }
        };

        return jcrop_api;
    };

    return crop;
};