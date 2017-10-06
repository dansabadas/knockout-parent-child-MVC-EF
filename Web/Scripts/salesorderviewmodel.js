var ObjectState = {
  Unchanged: 0,
  Added: 1,
  Modified: 2,
  Deleted: 3
};


var salesOrderItemMapping = { // this is the mapping for the 'SalesOrderItems' collection subproperty of the parent SalesOrderViewModel
  'SalesOrderItems': {
    key: function (salesOrderItem) {
      return ko.utils.unwrapObservable(salesOrderItem.SalesOrderItemId);
    },
    create: function (options) {
      return new SalesOrderItemViewModel(options.data);
    }
  }
};

SalesOrderItemViewModel = function(data) {
  var self = this;
  ko.mapping.fromJS(data, {}, self);

  self.flagSalesOrderItemAsEdited = function() {
    if (self.ObjectState() !== ObjectState.Added) {
      self.ObjectState(ObjectState.Modified);
    }

    return true;
  };

  self.ExtendedPrice = ko.computed(function() {
    return (self.Quantity() * self.UnitPrice()).toFixed(2);
  });
};

SalesOrderViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

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
      if (self.ObjectState() !== ObjectState.Added) {
        self.ObjectState(ObjectState.Modified);
      }

      return true;  // tell ko you want the default action of the UI control that raised the event with this handler
    };

    self.addSalesOrderItem = function () {
      var salesOrderItem = new SalesOrderItemViewModel({ SalesOrderItemId: 0, ProductCode: "", Quantity: 1, UnitPrice: 0, ObjectState: ObjectState.Added });
      self.SalesOrderItems.push(salesOrderItem);
    };

    self.Total = ko.computed(function () {
      var total = 0;
      ko.utils.arrayForEach(self.SalesOrderItems(), function (salesOrderItem) {
        total += parseFloat(salesOrderItem.ExtendedPrice());
      });
      return total.toFixed(2);
    });

    self.deleteSalesOrderItem = function (salesOrderItem) {
      self.SalesOrderItems.remove(this);  // self.SalesOrderItems is ko property but still can access array-ish methods like remove; this === salesOrderItem

      if (salesOrderItem.SalesOrderItemId() > 0 && self.SalesOrderItemsToDelete.indexOf(salesOrderItem.SalesOrderItemId()) === -1)
        self.SalesOrderItemsToDelete.push(salesOrderItem.SalesOrderItemId());
    };
};