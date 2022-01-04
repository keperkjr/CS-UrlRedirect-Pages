class UnobtrusiveAjaxHandler {
    static getNew(parentContainerSelector) {
        let handler = {};
        handler.$parent = $(parentContainerSelector);

        handler.onSuccess = (result) => {
            if (result.redirectTo) {
                // The operation was a success on the server as it returned
                // a JSON object with an url property pointing to the location
                // you would like to redirect to => now use the window.location.href
                // property to redirect the client to this location
                //alert('Redirecting');
                window.location.href = result.redirectTo;
            } else {
                // The server returned a partial view => let's refresh
                // the corresponding section of our DOM with it
                //alert('Reloading');
                //console.log(result);
                handler.$parent.html(result);
            }
        }
        handler.onFailure = (xhr) => {}
        handler.onComplete = (xhr) => {};
        return handler;
    }
}