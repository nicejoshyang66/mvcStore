
function initLogin() {
    const host = document.getElementById('login-app');
    const token = host.querySelector('input[name="__RequestVerificationToken"]').value;

    const app = Vue.createApp({

        setup() {
            const userId = Vue.ref('');
            const password = Vue.ref('');
            const rememberMe = Vue.ref(false);
            const busy = Vue.ref(false);
            const error = Vue.ref('');

            async function mysubmit() {
                // 正在登入時，不重複送出表單。
                if (busy.value) {
                    return;
                }

                busy.value = true;
                error.value = '';

                try {
                    // 整理表單資料，再送給伺服器。
                    const loginData = {
                        userId: userId.value,
                        password: password.value,
                        rememberMe: rememberMe.value
                    };
                    // await 會等待伺服器回應，再繼續下一個步驟。
                    const response = await fetch(host.dataset.loginUrl, {
                        method: 'POST',
                        credentials: 'same-origin',
                        headers: {
                            'Content-Type': 'application/json',
                            RequestVerificationToken: token
                        },
                        body: JSON.stringify(loginData)
                    });
                    // 如果回應不是有效的 JSON，先使用空物件。
                    let result = {};
                    try {
                        result = await response.json();
                    } catch (error) {
                        result = {};
                    }

                    if (!response.ok) {
                        throw new Error(result.message || '登入失敗，請重新整理頁面再試。');
                    }

                    // 清除側邊欄紀錄；瀏覽器不允許存取時，仍繼續登入。
                    try {
                        sessionStorage.removeItem('sidebar-first-open-done');
                    } catch (error) {
                        // 此紀錄不影響登入，不需要顯示錯誤。
                    }

                    window.location.assign(result.redirectUrl);

                }

                catch (requestError) {
                    error.value = requestError.message || '無法連線，請稍後再試。';
                }

                finally {
                    // 無論成功或失敗，都結束等待狀態。
                    busy.value = false;
                }
            }

            return { userId, password, rememberMe, busy, error, mysubmit };
        }
    });

    app.mount(host);

}

// 呼叫函式，初始化登入畫面。
initLogin();

