import * as React from 'react'
import { JS, ConsoleLogger as Logger } from '@aws-amplify/core'
import Auth from '@aws-amplify/auth'
import AuthPiece, { IAuthPieceProps, IAuthPieceState } from './AuthPiece'
import TOTPSetupComp from '../Widget/TOTPSetupComp'

import { auth } from '../Amplify-UI/data-test-attributes'

const logger = new Logger('TOTPSetup')

export default class TOTPSetup extends AuthPiece<IAuthPieceProps, IAuthPieceState> {
    constructor(props: IAuthPieceProps) {
        super(props)

        this._validAuthStates = ['TOTPSetup']
        this.onTOTPEvent = this.onTOTPEvent.bind(this)
        this.checkContact = this.checkContact.bind(this)
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

    onTOTPEvent(event: any, data: any, user: any) {
        logger.debug('on totp event', event, data)
        // const user = this.props.authData;
        if (event === 'Setup TOTP') {
            if (data === 'SUCCESS') {
                this.checkContact(user)
            }
        }
    }

    showComponent(theme: any) {
        const { hide } = this.props
        if (hide && hide.includes(TOTPSetup)) {
            return null
        }

        return <TOTPSetupComp {...this.props} onTOTPEvent={this.onTOTPEvent} data-test={auth.TOTPSetup.component} />
    }
}
