let howl = null;
let soundId = null;

window.howl = {
    play: function (dotnetReference, options) {
        if (howl) {
            stop();
        }

        howl = new Howl({
            src: options.sources,
            format: options.formats,
            html5: options.html5,
            onplay: async function (id) {
                const duration = Math.round(howl.duration());
                await dotnetReference.invokeMethodAsync('OnPlayCallback', id, duration);
            },
            onstop: async function (id) {
                await dotnetReference.invokeMethodAsync('OnStopCallback', id);
            },
            onpause: async function (id) {
                await dotnetReference.invokeMethodAsync('OnPauseCallback', id);
            },
            onrate: async function (id) {
                const currentRate = howl.rate();
                await dotnetReference.invokeMethodAsync('OnRateCallback', id, currentRate);
            },
            onend: async function (id) {
                await dotnetReference.invokeMethodAsync('OnEndCallback', id);
            },
            onload: async function () {
                await dotnetReference.invokeMethodAsync('OnLoadCallback');
            },
            onloaderror: async function (id, error) {
                await dotnetReference.invokeMethodAsync('OnLoadErrorCallback', id, error);
            },
            onplayerror: async function (id, error) {
                await dotnetReference.invokeMethodAsync('OnPlayErrorCallback', id, error);
            }
        });

        soundId = howl.play();
        return soundId;
    },
    stop: function () {
        if (howl) {
            howl.stop();
        }

        soundId = null;
        howl = null;
    },
    pause: function (id) {
        if (howl) {
            if (howl.playing()) {
                if (id) {
                    howl.pause(id);
                } else {
                    howl.pause();
                }
            } else {
                howl.play(soundId);
            }
        }
    },
    seek: function (position) {
        if (howl) {
            howl.seek(position);
        }
    },
    rate: function (rate) {
        if (howl) {
            howl.rate(rate);
        }
    },
    load: function () {
        if (howl) {
            howl.load();
        }
    },
    unload: function () {
        if (howl) {
            howl.unload();

            soundId = null;
            howl = null;
        }
    },
    getIsPlaying: function () {
        if (howl) {
            return howl.playing();
        }

        return false;
    },
    getRate: function () {
        if (howl) {
            return howl.rate();
        }

        return 0;
    },
    getCurrentTime: function () {
        if (howl && howl.playing()) {
            const seek = howl.seek();
            return Math.round(seek || 0);
        }

        return 0;
    },
    getTotalTime: function () {
        if (howl) {
            const duration = howl.duration();

            console.log(duration);

            return Math.round(duration || 0);
        }

        return 0;
    }
};

window.howler = {
    mute: function (muted) {
        Howler.mute(muted);
    },
    getCodecs: function () {
        const codecs = [];
        for (let [key, value] of Object.entries(Howler._codecs)) {
            if (value) {
                codecs.push(key);
            }
        }

        return codecs.sort();
    },
    isCodecSupported: function (extension) {
        return extension ? Howler._codecs[extension.replace(/^x-/, '')] : false;
    }
};