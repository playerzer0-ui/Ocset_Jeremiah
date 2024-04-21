const staticCacheName = 'site-static-v2';
const dynamicCacheName = 'site-dynamic-v1';
const assets = [
    '/lib/bootstrap/dist/css/bootstrap.min.css',
    '/lib/bootstrap/dist/js/bootstrap.bundle.min.js',
    '/lib/jquery/dist/jquery.min.js',
    '/js/app.js',
    '/js/site.js?v=PolN_KPS76wvg4s6Jd1jcKHS-q50NDMy4DFZH4y22eY',
    '/Customers/Login',
    '/Customers/Register',
    '/Home/Fallback',
    '/css/styles.css?v=yKtIB1H3dic61ZH37T0UEx7hjJZXquDkLc45ypkgAeY',
    '/css/site.css?v=omRbCddzb4aoq0bPPPp1iIUFO8KY9uW53yx8xPUwDQE',

];

const limitCacheSize = (name, size) => {
    caches.open(name).then(cache => {
        cache.keys().then(keys => {
            if (keys.length > size) {
                cache.delete(keys[0]).then(limitCacheSize(name, size));
            }
        });
    });
};

// install event
self.addEventListener('install', evt => {
    //console.log('service worker installed');
    evt.waitUntil(
        caches.open(staticCacheName).then((cache) => {
            console.log('caching shell assets');
            cache.addAll(assets).then(r => console.log("added all")).catch(err => console.log("error", err));
        })
    );
});

// activate event
self.addEventListener('activate', evt => {
    //console.log('service worker activated');
    evt.waitUntil(
        caches.keys().then(keys => {
            //console.log(keys);
            return Promise.all(keys
                .filter(key => key !== staticCacheName && key !== dynamicCacheName)
                .map(key => caches.delete(key))
            );
        })
    );
});

// fetch event
self.addEventListener('fetch', evt => {
    evt.respondWith(
        caches.match(evt.request).then(cacheRes => {
            return cacheRes || fetch(evt.request).then(fetchRes => {
                return caches.open(dynamicCacheName).then(cache => {
                    cache.put(evt.request.url, fetchRes.clone());
                    limitCacheSize(dynamicCacheName, 15);
                    return fetchRes;
                });
            }).catch(() => {
                // Network request failed, redirect to fallback.jsp
                return Response.redirect('/Home/Fallback', 302);
            });
        }).catch(() => {
            // Cache match failed, redirect to fallback.jsp
            return Response.redirect('/Home/Fallback', 302);
        })
    );
});
