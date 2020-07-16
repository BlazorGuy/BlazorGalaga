let addPath = false;
let resetAnimation = false;
let spriteSheetLoaded = false;

window.initFromBlazor = (instance) => {

    resizeCanvas();

    window.instance = instance;

    document.getElementById("imgSpriteSheet").addEventListener('load', function (e) {

        var canvas = document.querySelector('#theCanvas canvas');

        canvas.addEventListener("mousemove", function (e) {
            var pos = getMousePos(canvas, e);
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnMouseMove', pos);
        });
        canvas.addEventListener("mousedown", function (e) {
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnMouseDown');
        });
        canvas.addEventListener("mouseup", function (e) {
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnMouseUp');
        });
        document.getElementById("btnAddPath").addEventListener('click', function (e) {
            addPath = true;
        });
        document.getElementById("btnResetAnimation").addEventListener('click', function (e) {
            resetAnimation = true;
        });
        document.addEventListener("keydown", function (e) {
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', e.code);
        });
        document.addEventListener("keyup", function (e) {
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyUp', e.code);
        });

        window.addEventListener("resize", resizeCanvas);

        window.instance.invokeMethodAsync('SpriteSheetLoaded');

        gameLoop(Date.now);

    });
};

function gameLoop(timeStamp) {

    var editcurveschecked = document.getElementById('EditCurves').checked;
    var pauseanimation = document.getElementById('PauseAnimation').checked;

    var gameloopobject = {
        editcurveschecked: editcurveschecked,
        timestamp: timeStamp,
        pauseanimation: pauseanimation,
        addpath: addPath,
        resetanimation: resetAnimation,
        spritesheetloaded: spriteSheetLoaded
    };

    window.instance.invokeMethodAsync('GameLoop', gameloopobject);

    addPath = false;
    resetAnimation = false;

    window.requestAnimationFrame(gameLoop);
}

function setImageSmoothingForCanvases() {

    var canvaslist = document.getElementsByTagName("canvas");

    for (let cnv of canvaslist) {
        var ctx = cnv.getContext("2d");
        ctx.imageSmoothingEnabled = false;
    }
}

function getMousePos(canvas, evt) {
    var rect = canvas.getBoundingClientRect(), // abs. size of element
        scaleX = canvas.width / rect.width,    // relationship bitmap vs. element for X
        scaleY = canvas.height / rect.height;  // relationship bitmap vs. element for Y

    return {
        x: (evt.clientX - rect.left) * scaleX,   // scale mouse coordinates after they have
        y: (evt.clientY - rect.top) * scaleY     // been adjusted to be relative to element
    }
}

window.logDiagnosticInfo = function (diagnosticinfo) {
    document.getElementById("divDiagnostics").innerHTML = diagnosticinfo;
}


function resizeCanvas() {

    setImageSmoothingForCanvases();

    var game = {
        canvas: document.getElementById("theCanvas"),
        width: 672,
        height: 944
    };

    var newGameWidth, newGameHeight;

    // Get the dimensions of the viewport
    var viewport = {
        width: window.innerWidth,
        height: window.innerHeight
    };

    // Determine game size
    const ratio = game.height / game.width;
    const viewportRatio = viewport.height / viewport.width;

    if (ratio > viewportRatio) {
        newGameHeight = viewport.height;
        newGameWidth = newGameHeight * game.width / game.height;
    } else {
        newGameWidth = viewport.width;
        newGameHeight = newGameWidth * game.height / game.width;
    }

    game.canvas.style.width = newGameWidth + "px";
    game.canvas.style.height = newGameHeight + "px";

    var paddingX = (viewport.width - newGameWidth) / 2;
    var paddingY = (viewport.height - newGameHeight) / 2;
    var margin = paddingY + "px " + paddingX + "px";

    // Set the new margin of the game so it will be centered
    game.canvas.style.margin = margin;

};

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}