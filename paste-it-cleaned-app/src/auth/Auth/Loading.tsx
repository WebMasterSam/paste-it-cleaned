import * as React from 'react'
import { I18n } from '@aws-amplify/core'

import AuthPiece, { IAuthPieceProps, IAuthPieceState } from './AuthPiece'
import { FormSection, SectionBody } from '../Amplify-UI/Amplify-UI-Components-React'

import { auth } from '../Amplify-UI/data-test-attributes'

export default class Loading extends AuthPiece<IAuthPieceProps, IAuthPieceState> {
    constructor(props: IAuthPieceProps) {
        super(props)

        this._validAuthStates = ['loading']
    }

    showComponent(theme: any) {
        const { hide } = this.props
        if (hide && hide.includes(Loading)) {
            return null
        }

        return (
            <FormSection theme={theme} data-test={auth.loading.section} className="auth-form">
                <SectionBody theme={theme}>{I18n.get('Loading...')}</SectionBody>
            </FormSection>
        )
    }
}
