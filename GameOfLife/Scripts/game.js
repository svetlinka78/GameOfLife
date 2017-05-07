(function () {
    var gameHub = $.connection.gameHub;
    //$.connection.hub.logging = true;
    $.connection.hub.start();

    gameHub.client.Message = function (message) { //property call back from server
        model.addMessage(message);
    };

    gameHub.client.newPoints = function (points) {
        model.addPoints(points);
    };
    
    gameHub.client.outputBoard = function (gamePoints) {
        model.addBoard(gamePoints);
    };

    var ChartEntry = function (name) {
        var self = this;
        self.name = name;
        self.chart = new SmoothieChart({ millisPerPixel: 50, labels: { fontSize: 15 } });
        self.timeSeries = new TimeSeries();
        self.chart.addTimeSeries(self.timeSeries, { lineWidth: 3, strokeStyle: '#00ff00' });

        self.start = function () {
            self.canvas = $("#" + name);
            self.chart.streamTo(self.canvas.get(0));
        };
    }

    var GameEntry = function (name) {
        var self = this;
        self.name = name;
        
        self.start = function () {
        self.canvas = $("#" + name).get(0);
        self.ctx = self.canvas.getContext("2d");
        self.cellsize = 10;
        self.gridsize = 0//gamePoints[0].size * cellsize;
        //self.ctx.canvas.width = self.ctx.canvas.height = self.gridsize;
        //self.ctx.strokeStyle = 'red';
        
        self.drawGrid = function (ctx, size, game) {
            var w = ctx.canvas.width,
                h = ctx.canvas.height;
            ctx.beginPath();
            for (var x = 0; x < w; x += size) {
                ctx.moveTo(x - 0.5, 0);
                ctx.lineTo(x - 0.5, h);
            }
            for (var y = 0; y < h; y += size) {
                ctx.moveTo(0, y - 0.5);
                ctx.lineTo(w, y - 0.5);
            }
            ctx.stroke();


           for (var x = 0; x < game.size; x++) {
                for (var y = 0; y < game.size; y++) {
                    var key = "\(" + x + ', ' + y + ")";
                    if (game.points[key] === true) {
                        ctx.fillRect(x * size - 0.5, y * size - 0.5, size, size);
                    }
                }
            }
        }
          
    };
}


    var Model = function ()
    {
        var self = this;
        self.message = ko.observable(""),
        self.messages = ko.observableArray(),
        self.points = ko.observableArray(),
        self.gpoints = ko.observableArray()
    }

    Model.prototype = {
        addPoints: function(updatedPoints){
            var self = this;
            $.each(updatedPoints,function(index,updatedPoints){
                var entry = ko.utils.arrayFirst(self.points(),function(points){
                    return points.name == updatedPoints.name;
                });
                if(!entry){
                    entry = new ChartEntry(updatedPoints.name);
                    self.points.push(entry);
                    entry.start();
                }
                entry.timeSeries.append(new Date().getTime(),updatedPoints.value);
            });
        },
        addMessage: function (message) {
            var self = this;
            self.messages.push(message);
        },
        sendMessage: function () {
            var self = this;
            gameHub.server.send(self.message());
            self.message("");
        },
        startGame: function () {
            //var self = this;
            gameHub.server.startGame();
        },
        stopGame: function () {
            gameHub.server.stopGame();
        },
        addBoard:function(gamePoints){
            //var ctx = document.querySelector('canvas').getContext('2d');

            var self = this;
            $.each(gamePoints, function (index, gamePoints) {
                var entry = ko.utils.arrayFirst(self.gpoints(),function(gpoints){
                    return gpoints.name == gamePoints.name;
                });
                if(!entry){
                    entry = new GameEntry(gamePoints.name);
                    self.gpoints.push(entry);
                    entry.start();
                }
                entry.gridsize = gamePoints.size * entry.cellsize;
                entry.ctx.canvas.width = entry.ctx.canvas.height = entry.gridsize;
                entry.ctx.canvas.strokeStyle = 'red';
                entry.drawGrid(entry.ctx, entry.cellsize, gamePoints);
               
          });
            
            
           
        }
       
    };



 


    var model = new Model();

    var init = function () {
        ko.applyBindings(model);
    };

    $(init);

}());


