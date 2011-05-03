(function($){
    var mobile = kendo.mobile = kendo.mobile || {};

    $.extend(mobile, {
        init: function(options) {
            var html = [ "<meta name='viewport' content='width=device-width' />" ];

            if (options.icon) {
                html.push("<link rel='apple-touch-icon' href='");
                html.push(options.icon);
                html.push("' />");
            }

            $(html.join("")).appendTo("head");
        }
    });

    function Application(options) {
        $.extend(this, options);
        this.views = [];
    }

    Application.prototype = {
        run: function(callback) {
            this.root = document.body;

            callback(this);
            this.show(0);

            window.scrollTo(0, 1);
        },

        addView: function(view) {
            this.views.push(view);
        },

        show: function(viewIndex, options) {
            if (this.currentView) {
                this.currentView.hide();
            }

            var view = this.views[viewIndex]; 
            
            view.show(this.root);

            this.currentView = view;
        }
    };

    // temporary, the class will be removed
    window.Application = Application;
})(jQuery);
