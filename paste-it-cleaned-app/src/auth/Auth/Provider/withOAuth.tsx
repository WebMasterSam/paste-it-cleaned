import * as React from 'react'

import { I18n } from '@aws-amplify/core'
import Auth from '@aws-amplify/auth'
import AmplifyTheme from '../../Amplify-UI/Amplify-UI-Theme'
import { oAuthSignInButton } from '@aws-amplify/ui'
import { SignInButton, SignInButtonContent } from '../../Amplify-UI/Amplify-UI-Components-React'

export default function withOAuth(Comp: any) {
    return class extends React.Component<any, any> {
        constructor(props: any) {
            super(props)
            this.signIn = this.signIn.bind(this)
        }

        signIn(_e: any, provider: any) {
            Auth.federatedSignIn({ provider })
        }

        render() {
            return <Comp {...this.props} OAuthSignIn={this.signIn} />
        }
    }
}

const Button = (props: any) => (
    <SignInButton id={oAuthSignInButton} onClick={() => props.OAuthSignIn()} theme={props.theme || AmplifyTheme} variant={'oAuthSignInButton'}>
        <SignInButtonContent theme={props.theme || AmplifyTheme}>{I18n.get(props.label || 'Sign in with AWS')}</SignInButtonContent>
    </SignInButton>
)

export const OAuthButton = withOAuth(Button)
