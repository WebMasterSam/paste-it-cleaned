import * as React from 'react'

import { I18n, ConsoleLogger as Logger } from '@aws-amplify/core'
import Auth from '@aws-amplify/auth'
import AmplifyTheme from '../../Amplify-UI/Amplify-UI-Theme'
import { facebookSignInButton } from '@aws-amplify/ui'
import { SignInButton, SignInButtonIcon, SignInButtonContent } from '../../Amplify-UI/Amplify-UI-Components-React'
import Constants from '../common/constants'

const logger = new Logger('withFacebook')

export default function withFacebook(Comp: any) {
    return class extends React.Component<any, any> {
        constructor(props: any) {
            super(props)

            this.fbAsyncInit = this.fbAsyncInit.bind(this)
            this.initFB = this.initFB.bind(this)
            this.signIn = this.signIn.bind(this)
            this.signOut = this.signOut.bind(this)
            this.federatedSignIn = this.federatedSignIn.bind(this)

            this.state = {}
        }

        signIn() {
            //@ts-ignore
            const fb = window.FB

            fb.getLoginStatus((response: any) => {
                const payload = {
                    provider: Constants.FACEBOOK,
                }
                try {
                    localStorage.setItem(Constants.AUTH_SOURCE_KEY, JSON.stringify(payload))
                } catch (e) {
                    logger.debug('Failed to cache auth source into localStorage', e)
                }

                if (response.status === 'connected') {
                    this.federatedSignIn(response.authResponse)
                } else {
                    fb.login(
                        (response: any) => {
                            if (!response || !response.authResponse) {
                                return
                            }
                            this.federatedSignIn(response.authResponse)
                        },
                        {
                            scope: 'public_profile,email',
                        }
                    )
                }
            })
        }

        federatedSignIn(response: any) {
            logger.debug(response)
            const { onStateChange } = this.props

            const { accessToken, expiresIn } = response
            const date = new Date()
            const expires_at = expiresIn * 1000 + date.getTime()
            if (!accessToken) {
                return
            }

            //@ts-ignore
            const fb = window.FB
            fb.api('/me', { fields: 'name,email' }, (response: any) => {
                const user = {
                    name: response.name,
                    email: response.email,
                }
                if (!Auth || typeof Auth.federatedSignIn !== 'function' || typeof Auth.currentAuthenticatedUser !== 'function') {
                    throw new Error('No Auth module found, please ensure @aws-amplify/auth is imported')
                }

                Auth.federatedSignIn('facebook', { token: accessToken, expires_at }, user)
                    .then(credentials => {
                        return Auth.currentAuthenticatedUser()
                    })
                    .then(authUser => {
                        if (onStateChange) {
                            onStateChange('signedIn', authUser)
                        }
                    })
            })
        }

        signOut() {
            //@ts-ignore
            const fb = window.FB
            if (!fb) {
                logger.debug('FB sdk undefined')
                return Promise.resolve()
            }

            fb.getLoginStatus((response: any) => {
                if (response.status === 'connected') {
                    return new Promise((res, rej) => {
                        logger.debug('facebook signing out')
                        fb.logout((response: any) => {
                            res(response)
                        })
                    })
                } else {
                    return Promise.resolve()
                }
            })
        }

        componentDidMount() {
            const { facebook_app_id } = this.props
            //@ts-ignore
            if (facebook_app_id && !window.FB) this.createScript()
        }

        fbAsyncInit() {
            logger.debug('init FB')

            const { facebook_app_id } = this.props
            //@ts-ignore
            const fb = window.FB
            fb.init({
                appId: facebook_app_id,
                cookie: true,
                xfbml: true,
                version: 'v2.11',
            })

            fb.getLoginStatus((response: any) => logger.debug(response))
        }

        initFB() {
            //@ts-ignore
            const fb = window.FB
            logger.debug('FB inited')
        }

        createScript() {
            //@ts-ignore
            window.fbAsyncInit = this.fbAsyncInit

            const script = document.createElement('script')
            script.src = 'https://connect.facebook.net/en_US/sdk.js'
            script.async = true
            script.onload = this.initFB
            document.body.appendChild(script)
        }

        render() {
            //@ts-ignore
            const fb = window.FB
            return <Comp {...this.props} fb={fb} facebookSignIn={this.signIn} facebookSignOut={this.signOut} />
        }
    }
}

const Button = (props: any) => (
    <SignInButton id={facebookSignInButton} onClick={props.facebookSignIn} theme={props.theme || AmplifyTheme} variant={'facebookSignInButton'}>
        <SignInButtonIcon theme={props.theme || AmplifyTheme}>
            <svg viewBox="0 0 279 538" xmlns="http://www.w3.org/2000/svg">
                <g id="Page-1" fill="none" fillRule="evenodd">
                    <g id="Artboard" fill="#FFF">
                        <path
                            d="M82.3409742,538 L82.3409742,292.936652 L0,292.936652 L0,196.990154 L82.2410458,196.990154 L82.2410458,126.4295 C82.2410458,44.575144 132.205229,0 205.252865,0 C240.227794,0 270.306232,2.59855099 279,3.79788222 L279,89.2502322 L228.536175,89.2502322 C188.964542,89.2502322 181.270057,108.139699 181.270057,135.824262 L181.270057,196.89021 L276.202006,196.89021 L263.810888,292.836708 L181.16913,292.836708 L181.16913,538 L82.3409742,538 Z"
                            id="Fill-1"
                        />
                    </g>
                </g>
            </svg>
        </SignInButtonIcon>
        <SignInButtonContent theme={props.theme || AmplifyTheme}>{I18n.get('Sign In with Facebook')}</SignInButtonContent>
    </SignInButton>
)

export const FacebookButton = withFacebook(Button)
