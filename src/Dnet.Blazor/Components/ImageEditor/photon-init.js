// Photon WASM initialization module
// This module handles lazy loading and initialization of the Photon WASM library

let photonModule = null;
let photonPromise = null;

/**
 * Initialize Photon WASM module
 * @returns {Promise} Promise that resolves when Photon is loaded
 */
async function initPhoton() {
    if (photonModule) {
        return photonModule;
    }

    if (photonPromise) {
        return photonPromise;
    }

    photonPromise = (async () => {
        try {
            // Load the Photon ES module from the static file location
            // The module path is relative to the app's base path
            const basePath = document.baseURI || window.location.origin + '/';
            const modulePath = new URL('photon_rs.js', basePath).href;
            
            console.log('🔄 Loading Photon WASM from:', modulePath);
            
            // Dynamic import of the Photon ES module
            const module = await import(/* webpackIgnore: true */ modulePath);
            
            // Initialize WASM with explicit path
            const wasmPath = new URL('photon_rs_bg.wasm', basePath).href;
            await module.default(wasmPath);
            
            photonModule = module;
            console.log('✅ Photon WASM initialized successfully');
            return module;
        } catch (error) {
            console.error('❌ Failed to initialize Photon WASM:', error);
            photonPromise = null;
            throw error;
        }
    })();

    return photonPromise;
}

/**
 * Get initialized Photon module
 * @returns {Promise} Promise that resolves to the Photon module
 */
async function getPhoton() {
    if (!photonModule) {
        await initPhoton();
    }
    return photonModule;
}

/**
 * Check if Photon is initialized
 * @returns {boolean} True if Photon is ready
 */
function isPhotonReady() {
    return photonModule !== null;
}

window.photonInit = {
    init: initPhoton,
    get: getPhoton,
    isReady: isPhotonReady
};
