var ObjectState = {
  Unchanged: 0,
  Added: 1,
  Modified: 2,
  Deleted: 3
};

SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, {}, self);

    self.save = function () {
      $.ajax({
        url: "/Sales/Save/",
        type: "POST",
        data: ko.toJSON(self),
        contentType: "application/json",
        success: function (data) {
          if (data.salesOrderViewModel != null)
            ko.mapping.fromJS(data.salesOrderViewModel, {}, self);

          if (data.newLocation != null)
            window.location = data.newLocation;
        }
      });
    }

    self.flagSalesOrderAsEdited = function () {
      console.log("flagged!");
      if (self.ObjectState() !== ObjectState.Added) {
        self.ObjectState(ObjectState.Modified);
      }

      return true;  // tell ko you want the default action of the UI control that raised the event with this handler
    };
};