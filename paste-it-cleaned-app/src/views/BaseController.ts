import Auth from '@aws-amplify/auth'
import { CurrentSession } from '../session/Session'
import backend from '../config/backend.json'

export class BaseController {
    callBackend = (url: string, method: string, success?: (json: any) => void, error?: (e: any) => void) => {
        Auth.currentSession()
            .then(v => v.getAccessToken().getJwtToken())
            .then(t => {
                fetch(url, {
                    method: method,
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*',
                        'X-Requested-With': 'XMLHttpRequest',
                        Authorization: `Bearer ${t}`,
                    },
                    mode: 'cors',
                    cache: 'no-cache',
                })
                    .then(res => res.json())
                    .then(t => {
                        success && success(t)
                    })
                    .catch(e => {
                        console.log(e)
                        error && error(e)
                    })
            })
    }

    callBackendWithoutResponse = (url: string, method: string, success?: () => void, error?: (e: any) => void) => {
        Auth.currentSession()
            .then(v => v.getAccessToken().getJwtToken())
            .then(t => {
                fetch(url, {
                    method: method,
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*',
                        'X-Requested-With': 'XMLHttpRequest',
                        Authorization: `Bearer ${t}`,
                    },
                    mode: 'cors',
                    cache: 'no-cache',
                })
                    .then(t => {
                        success && success()
                    })
                    .catch(e => {
                        console.log(e)
                        error && error(e)
                    })
            })
    }

    callBackendWithPayload = (url: string, method: string, payload: any, success?: (json: any) => void, error?: (e: any) => void) => {
        Auth.currentSession()
            .then(v => v.getAccessToken().getJwtToken())
            .then(t => {
                fetch(url, {
                    method: method,
                    body: JSON.stringify(payload),
                    headers: {
                        'Content-Type': 'application/json',
                        'Access-Control-Allow-Origin': '*',
                        'X-Requested-With': 'XMLHttpRequest',
                        Authorization: `Bearer ${t}`,
                    },
                    mode: 'cors',
                    cache: 'no-cache',
                })
                    .then(res => res.json())
                    .then(t => {
                        success && success(t)
                    })
                    .catch(e => {
                        console.log(e)
                        error && error(e)
                    })
            })
    }

    culturedEndpoint(path: string) {
        if (path.indexOf('?') === -1) {
            return backend.endpoints.base + path + '?culture=' + CurrentSession.Culture
        } else {
            return backend.endpoints.base + path + '&culture=' + CurrentSession.Culture
        }
    }
}
