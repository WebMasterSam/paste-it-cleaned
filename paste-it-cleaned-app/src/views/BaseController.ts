import Auth from '@aws-amplify/auth'
import { CurrentSession } from '../session/Session'

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

    culturedEndpoint(url: string) {
        return url + '?culture=' + CurrentSession.Culture
    }
}
