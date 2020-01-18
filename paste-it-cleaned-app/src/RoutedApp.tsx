import React from 'react'
import { withRouter, RouteComponentProps } from 'react-router-dom'
import { I18nextProvider, initReactI18next, withTranslation } from 'react-i18next'
import i18next from 'i18next'
import App from './App'
import LanguageDetector from 'i18next-browser-languagedetector'

import en_US from './i18n/en/app.json'
import fr_CA from './i18n/fr/app.json'

i18next
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        interpolation: { escapeValue: false },
        lng: 'en',
        fallbackLng: 'en',
        ns: ['app'],
        defaultNS: 'app',
        resources: {
            en: { app: en_US },
            fr: { app: fr_CA },
        },
        react: {
            wait: true,
        },
    })

export class RoutedApp extends React.Component<RouteComponentProps & { i18n: any; tReady: any; t: any }> {
    render() {
        return (
            <I18nextProvider i18n={i18next}>
                <App />
            </I18nextProvider>
        )
    }
}

export default withTranslation()(withRouter(RoutedApp))
