// 上方導覽列：處理登出。
const header = document.getElementById('header-app');
const tokenInput = header.querySelector('input[name="__RequestVerificationToken"]');
const token = tokenInput.value;

const headerApp = Vue.createApp({
    setup() {
        const busy = Vue.ref(false);
        const error = Vue.ref('');

        async function logout() {
            // 避免重複送出登出請求。
            if (busy.value) {
                return;
            }

            busy.value = true;
            error.value = '';

            try {
                const response = await fetch(header.dataset.logoutUrl, {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: {
                        RequestVerificationToken: token
                    }
                });

                if (!response.ok) {
                    throw new Error('登出失敗，請重新整理後再試。');
                }

                const result = await response.json();

                try {
                    sessionStorage.removeItem('sidebar-first-open-done');
                } catch (storageError) {
                    // 瀏覽器若禁止使用儲存空間，仍繼續登出。
                }

                location.assign(result.redirectUrl);
            } catch (requestError) {
                error.value = requestError.message;
            } finally {
                busy.value = false;
            }
        }

        return { busy, error, logout };
    }
});

headerApp.mount(header);

// 左側選單：載入登入者資料，以及管理選單的展開狀態。
const host = document.getElementById('menu-app');

const menuApp = Vue.createApp({

    setup() {
        const userId = Vue.ref('');
        const userName = Vue.ref('');
        const menu = Vue.ref([]);
        const opened = Vue.ref([]); // 存放已展開的主選單 ID。
        const loading = Vue.ref(true);
        const error = Vue.ref('');

        function isActive(url) {

            if (!url || url === '#') {
                return false;
            }

            const target = new URL(url, location.origin);
            const samePath = target.pathname.toLowerCase() === location.pathname.toLowerCase();
            const sameQuery = target.search === location.search;

            return samePath && sameQuery;

        }

        function toggle(id) {
            const index = opened.value.indexOf(id);

            if (index === -1) {
                // 尚未展開：加入 ID。
                opened.value.push(id);
            } else {
                // 已經展開：移除 ID，收合選單。
                opened.value.splice(index, 1);
            }
        }

        async function load() {
            loading.value = true;
            error.value = '';

            try {
                const response = await fetch(host.dataset.menuUrl, {
                    credentials: 'same-origin'
                });

                if (response.status === 401) {
                    location.assign(host.dataset.loginUrl);
                    return;
                }

                if (!response.ok) {
                    throw new Error('選單載入失敗，請稍後重試。');
                }

                const result = await response.json();
                userId.value = result.userId || '';
                userName.value = result.userName || '';
                menu.value = result.menu;
                opened.value = [];

                // 找出目前頁面所屬的主選單，並自動展開。
                for (let i = 0; i < menu.value.length; i++) {
                    const parent = menu.value[i];

                    for (let j = 0; j < parent.children.length; j++) {
                        const child = parent.children[j];

                        if (isActive(child.url)) {
                            opened.value.push(parent.id);
                            break;
                        }
                    }
                }

                // 同一分頁第一次載入時，自動展開第一個主選單。
                try {
                    const firstOpenDone = sessionStorage.getItem('sidebar-first-open-done');

                    if (!firstOpenDone && menu.value.length > 0) {
                        const firstMenuId = menu.value[0].id;

                        if (!opened.value.includes(firstMenuId)) {
                            opened.value.push(firstMenuId);
                        }

                        sessionStorage.setItem('sidebar-first-open-done', 'true');
                    }
                } catch (storageError) {
                    // 瀏覽器若禁止使用儲存空間，仍顯示已載入的選單。
                }
            } catch (requestError) {
                error.value = requestError.message;
            } finally {
                loading.value = false;
            }
        }

        Vue.onMounted(load);

        return { userId, userName, menu, opened, loading, error, isActive, toggle, load };
    }
});

menuApp.mount(host);
