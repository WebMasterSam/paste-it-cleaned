import * as React from 'react'

import { I18n, JS } from '@aws-amplify/core'
import Auth from '@aws-amplify/auth'

import AuthPiece, { IAuthPieceProps, IAuthPieceState } from './AuthPiece'
import { FormSection, FormField, SectionHeader, SectionBody, SectionFooter, Input, InputLabel, Button, Link, SectionFooterPrimaryContent, SectionFooterSecondaryContent } from '../Amplify-UI/Amplify-UI-Components-React'

import { auth } from '../Amplify-UI/data-test-attributes'

export interface IConfirmSignInState extends IAuthPieceState {
    mfaType: string
}

export default class ConfirmSignIn extends AuthPiece<IAuthPieceProps, IConfirmSignInState> {
    constructor(props: IAuthPieceProps) {
        super(props)

        this._validAuthStates = ['confirmSignIn']
        this.confirm = this.confirm.bind(this)
        this.checkContact = this.checkContact.bind(this)
        this.state = {
            mfaType: 'SMS',
        }
    }

    checkContact(user: any) {
        if (!Auth || typeof Auth.verifiedContact !== 'function') {
            throw new Error('No Auth module found, please ensure @aws-amplify/auth is imported')
        }

        Auth.verifiedContact(user).then(data => {
            if (!JS.isEmpty(data.verified)) {
                this.changeState('signedIn', user)
            } else {
                const newUser = Object.assign(user, data)
                this.changeState('verifyContact', newUser)
            }
        })
    }

    confirm(event: any) {
        if (event) {
            event.preventDefault()
        }
        const user = this.props.authData
        const { code } = this.inputs
        const mfaType = user.challengeName === 'SOFTWARE_TOKEN_MFA' ? 'SOFTWARE_TOKEN_MFA' : null
        if (!Auth || typeof Auth.confirmSignIn !== 'function') {
            throw new Error('No Auth module found, please ensure @aws-amplify/auth is imported')
        }

        Auth.confirmSignIn(user, code, mfaType)
            .then(() => {
                this.checkContact(user)
            })
            .catch(err => this.error(err))
    }

    componentDidUpdate() {
        // logger.debug('component did update with props', this.props);
        const user = this.props.authData
        const mfaType = user && user.challengeName === 'SOFTWARE_TOKEN_MFA' ? 'TOTP' : 'SMS'
        if (this.state.mfaType !== mfaType) this.setState({ mfaType })
    }

    showComponent(theme: any) {
        const { hide } = this.props
        if (hide && hide.includes(ConfirmSignIn)) {
            return null
        }

        return (
            <FormSection theme={theme} data-test={auth.confirmSignIn.section} className="auth-form">
                <SectionHeader theme={theme} data-test={auth.confirmSignIn.headerSection}>
                    {I18n.get('Confirm ' + this.state.mfaType + ' Code')}
                </SectionHeader>
                <form onSubmit={this.confirm} data-test={auth.confirmSignIn.bodySection}>
                    <SectionBody theme={theme}>
                        <FormField theme={theme}>
                            <InputLabel theme={theme}>{I18n.get('Code')} *</InputLabel>
                            <Input autoFocus placeholder={I18n.get('Code')} theme={theme} key="code" name="code" autoComplete="off" onChange={this.handleInputChange} data-test={auth.confirmSignIn.codeInput} />
                        </FormField>
                    </SectionBody>
                    <SectionFooter theme={theme}>
                        <SectionFooterPrimaryContent theme={theme} data-test={auth.confirmSignIn.confirmButton}>
                            <Button theme={theme} type="submit">
                                {I18n.get('Confirm')}
                            </Button>
                        </SectionFooterPrimaryContent>
                        <SectionFooterSecondaryContent theme={theme}>
                            <Link theme={theme} onClick={() => this.changeState('signIn')} data-test={auth.confirmSignIn.backToSignInLink}>
                                {I18n.get('Back to Sign In')}
                            </Link>
                        </SectionFooterSecondaryContent>
                    </SectionFooter>
                </form>
            </FormSection>
        )
    }
}
