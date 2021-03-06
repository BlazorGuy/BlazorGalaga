﻿let addPath = false;
let resetAnimation = false;
let spriteSheetLoaded = false;
let killBugs = false;
let captureShip = false;
let morphBug = false;
let hammertime = null;
let mobilemoving = false;
let mobileOptions = { time: 50 };

window.initFromBlazor = (instance) => {

    resizeCanvas();

    window.instance = instance;

    hammertime = new Hammer(document.getElementById("theCanvas"));

    //mobile support

    hammertime.on('tap', function (ev) {

        if (mobilemoving) {
            mobilemoving = false;
            if (ev.srcEvent.offsetX <= 260)
                DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyUp', "ArrowLeft");
            else
                DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyUp', "ArrowRight");
        }
        else {
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', "Space");
            mobilemoving = true;
            if (ev.srcEvent.offsetY <= 350) {
                return;
            }
            if (ev.srcEvent.offsetX <= 260)
                DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', "ArrowLeft");
            else
                DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', "ArrowRight");
        }
    });

    hammertime.on('press', function (ev, mobileOptions) {
        if (ev.srcEvent.offsetX <= 260)
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', "ArrowLeft");
        else
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyDown', "ArrowRight");
    });
    hammertime.on('pressup', function (ev, mobileOptions) {
        if (ev.srcEvent.offsetX <= 260)
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyUp', "ArrowLeft");
        else
            DotNet.invokeMethodAsync('BlazorGalaga', 'OnKeyUp', "ArrowRight");
    });


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
        document.getElementById("btnKillAllBugs").addEventListener('click', function (e) {
            killBugs = true;
        });
        document.getElementById("btnCaptureShip").addEventListener('click', function (e) {
            captureShip = true;
            document.getElementById("btnCaptureShip").disabled = true;
        });
        document.getElementById("btnMorphBug").addEventListener('click', function (e) {
            //document.getElementById("btnMorphBug").disabled = true;
            morphBug = true;
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
        killbugs: killBugs,
        resetanimation: resetAnimation,
        spritesheetloaded: spriteSheetLoaded,
        captureship: captureShip,
        morphbug: morphBug
    };

    window.instance.invokeMethodAsync('GameLoop', gameloopobject);

    killBugs = false;
    resetAnimation = false;
    captureShip = false;
    morphBug = false;

    window.requestAnimationFrame(gameLoop);
}

function setImageSmoothingForCanvases() {

    var canvaslist = document.getElementsByTagName("canvas");

    for (let cnv of canvaslist) {
        var ctx = cnv.getContext("2d");
        ctx.imageSmoothingEnabled = false;
        //ctx.globalCompositeOperation = 'destination-over';
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
    game.canvas.tabindex = 1;
    game.canvas.focus();

};
