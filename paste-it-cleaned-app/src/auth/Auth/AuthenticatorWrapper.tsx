import * as React from 'react'

import Authenticator from './Authenticator'

export { default as Authenticator } from './Authenticator'
export { default as AuthPiece } from './AuthPiece'
export { default as SignIn } from './SignIn'
export { default as ConfirmSignIn } from './ConfirmSignIn'
export { default as SignOut } from './SignOut'
export { default as RequireNewPassword } from './RequireNewPassword'
export { default as SignUp } from './SignUp'
export { default as ConfirmSignUp } from './ConfirmSignUp'
export { default as VerifyContact } from './VerifyContact'
export { default as ForgotPassword } from './ForgotPassword'
export { default as FederatedSignIn, FederatedButtons } from './FederatedSignIn'
export { default as TOTPSetup } from './TOTPSetup'
export { default as Loading } from './Loading'

export * from './Provider'

export function withAuthenticator(Comp: any, includeGreetings = false, authenticatorComponents = [], federated = null, theme = null, signUpConfig = {}, usernameAttributes: string[] = []) {
    return class extends React.Component<any, any> {
        public authConfig: any

        constructor(props: any) {
            super(props)

            this.handleAuthStateChange = this.handleAuthStateChange.bind(this)

            this.state = {
                authState: props.authState || null,
                authData: props.authData || null,
            }

            this.authConfig = {}

            if (typeof includeGreetings === 'object' && includeGreetings !== null) {
                this.authConfig = Object.assign(this.authConfig, includeGreetings)
            } else {
                this.authConfig = {
                    includeGreetings,
                    authenticatorComponents,
                    federated,
                    theme,
                    signUpConfig,
                    usernameAttributes,
                }
            }
        }

        handleAuthStateChange(state: any, data: any) {
            this.setState({ authState: state, authData: data })
        }

        render() {
            const { authState, authData } = this.state
            const signedIn = authState === 'signedIn'

            if (signedIn) {
                return (
                    <React.Fragment>
                        <Comp {...this.props} authState={authState} authData={authData} onStateChange={this.handleAuthStateChange} />
                    </React.Fragment>
                )
            }

            return (
                <div className="authenticator">
                    <Authenticator
                        {...this.props}
                        theme={this.authConfig.theme}
                        federated={this.authConfig.federated || this.props.federated}
                        hideDefault={this.authConfig.authenticatorComponents && this.authConfig.authenticatorComponents.length > 0}
                        signUpConfig={this.authConfig.signUpConfig}
                        usernameAttributes={this.authConfig.usernameAttributes}
                        onStateChange={this.handleAuthStateChange}
                        children={this.authConfig.authenticatorComponents || []}
                    />
                </div>
            )
        }
    }
}

export class AuthenticatorWrapper extends React.Component {
    constructor(props: any) {
        super(props)

        this.state = { auth: 'init' }

        this.handleAuthState = this.handleAuthState.bind(this)
        this.renderChildren = this.renderChildren.bind(this)
    }

    handleAuthState(state: any, data: any) {
        this.setState({ auth: state, authData: data })
    }

    renderChildren() {
        // @ts-ignore
        return this.props.children(this.state.auth)
    }

    render() {
        return (
            <div className="authenticator">
                <Authenticator {...this.props} onStateChange={this.handleAuthState} />
                {this.renderChildren()}
            </div>
        )
    }
}
