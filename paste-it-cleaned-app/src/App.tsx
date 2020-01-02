import React from 'react'
import Amplify, { Auth } from 'aws-amplify'
import { BrowserRouter, Switch, Route } from 'react-router-dom'

import { createMuiTheme, createStyles, ThemeProvider, withStyles, WithStyles } from '@material-ui/core/styles'
import CssBaseline from '@material-ui/core/CssBaseline'
import Hidden from '@material-ui/core/Hidden'

import Navigator from './components/Navigator'
import Header from './components/Header'
import AccountInformation from './views/account/information/AccountInformation'
import Footer from './components/Footer'

import { withAuthenticator } from './auth/Auth/AuthenticatorWrapper'
import { Url } from './shared/Urls'
import ApiKeys from './views/plugin/api-key/ApiKeys'
import BillingInformation from './views/account/billing/BillingInformation'
import AnalyticsHits from './views/analytics/hits/AnalyticsHits'
import PluginConfig from './views/plugin/config/PluginConfig'
import PluginIntegration from './views/plugin/integration/PluginIntegration'
import AnalyticsUsage from './views/analytics/usage/AnalyticsUsage'
import Dashboard from './views/dashboard/Dashboard'
import PageWrapper from './components/PageWrapper'
import { CurrentSession, replaceCurrentSession } from './session/Session'

import * as amplifyConfig from './config/aws-amplify.json'

import './App.less'

let theme = createMuiTheme({
    palette: {
        primary: {
            light: '#63ccff',
            main: '#009be5',
            dark: '#006db3',
        },
    },
    typography: {
        h2: {
            fontWeight: 500,
            fontSize: 20,
            letterSpacing: 0.5,
        },
        h5: {
            fontWeight: 500,
            fontSize: 26,
            letterSpacing: 0.5,
        },
    },
    shape: {
        borderRadius: 8,
    },
    props: {
        MuiTab: {
            disableRipple: true,
        },
    },
    mixins: {
        toolbar: {
            minHeight: 48,
        },
    },
})

theme = {
    ...theme,
    overrides: {
        MuiDrawer: {
            paper: {
                backgroundImage: 'linear-gradient(to right, #00324a, #003c59)',
            },
        },
        MuiButton: {
            label: {
                textTransform: 'none',
            },
            contained: {
                boxShadow: 'none',
                '&:active': {
                    boxShadow: 'none',
                },
            },
        },
        MuiTabs: {
            root: {
                marginLeft: theme.spacing(1),
            },
            indicator: {
                height: 3,
                borderTopLeftRadius: 3,
                borderTopRightRadius: 3,
                backgroundColor: theme.palette.common.white,
            },
        },
        MuiTab: {
            root: {
                textTransform: 'none',
                margin: '0 16px',
                minWidth: 0,
                padding: 0,
                [theme.breakpoints.up('md')]: {
                    padding: 0,
                    minWidth: 0,
                },
            },
        },
        MuiIconButton: {
            root: {
                padding: theme.spacing(1),
            },
        },
        MuiTooltip: {
            tooltip: {
                borderRadius: 4,
            },
        },
        MuiDivider: {
            root: {
                backgroundColor: '#404854',
            },
        },
        MuiListItemText: {
            primary: {
                fontWeight: theme.typography.fontWeightMedium,
            },
        },
        MuiListItemIcon: {
            root: {
                color: 'inherit',
                marginRight: 0,
                '& svg': {
                    fontSize: 20,
                },
            },
        },
        MuiAvatar: {
            root: {
                width: 32,
                height: 32,
            },
        },
    },
}

const drawerWidth = 256

const styles = createStyles({
    root: {
        display: 'flex',
        minHeight: '100vh',
    },
    drawer: {
        [theme.breakpoints.up('sm')]: {
            width: drawerWidth,
            flexShrink: 0,
        },
    },
    app: {
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
    },
    main: {
        flex: 1,
        background: '#eaeff1',
    },
})

export interface AppProps extends WithStyles<typeof styles> {}

export interface AppState {
    mobileOpen: boolean
}

Amplify.configure({
    Auth: {
        mandatorySignIn: true,
        region: amplifyConfig.cognito.REGION,
        userPoolId: amplifyConfig.cognito.USER_POOL_ID,
        identityPoolId: amplifyConfig.cognito.IDENTITY_POOL_ID,
        userPoolWebClientId: amplifyConfig.cognito.APP_CLIENT_ID,
    },
    Storage: {
        region: amplifyConfig.s3.REGION,
        bucket: amplifyConfig.s3.BUCKET,
        identityPoolId: amplifyConfig.cognito.IDENTITY_POOL_ID,
    },
    API: {
        endpoints: [
            {
                name: 'notes',
                endpoint: amplifyConfig.apiGateway.URL,
                region: amplifyConfig.apiGateway.REGION,
            },
        ],
    },
})

class App extends React.Component<AppProps, AppState> {
    constructor(props: AppProps) {
        super(props)
        this.state = { mobileOpen: false }
    }

    componentDidMount() {
        Auth.currentAuthenticatedUser()
            .then((value: any) => {
                const isValid: boolean = true
                const userName: string = value.username!
                const firstName: string = value.attributes!.name
                const lastName: string = value.attributes!.family_name
                const email: string = value.attributes!.email
                const culture: string = 'en-US'
                replaceCurrentSession(isValid, userName, firstName, lastName, email, culture)
                console.log(CurrentSession)
            })
            .catch(e => {
                replaceCurrentSession(false, '', '', '', '', 'en-US')
            })
    }

    render() {
        const { classes } = this.props

        const handleDrawerToggle = () => {
            this.setState({ mobileOpen: !this.state.mobileOpen })
        }

        return (
            <BrowserRouter>
                <ThemeProvider theme={theme}>
                    <div className={classes.root}>
                        <CssBaseline />

                        <nav className={classes.drawer}>
                            <Hidden smUp implementation="js">
                                <Navigator PaperProps={{ style: { width: drawerWidth } }} variant="temporary" open={this.state.mobileOpen} onClose={handleDrawerToggle} />
                            </Hidden>
                            <Hidden xsDown implementation="css">
                                <Navigator PaperProps={{ style: { width: drawerWidth } }} />
                            </Hidden>
                        </nav>

                        <div className={classes.app}>
                            <Header onDrawerToggle={handleDrawerToggle} />
                            <main className={classes.main}>
                                <Switch>
                                    <Route exact path={Url.Dashboard}>
                                        <Dashboard />
                                    </Route>
                                    <Route path={Url.Account}>
                                        <PageWrapper>
                                            <AccountInformation />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.ApiKey}>
                                        <PageWrapper>
                                            <ApiKeys />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.Billing}>
                                        <PageWrapper>
                                            <BillingInformation />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.Hits}>
                                        <PageWrapper>
                                            <AnalyticsHits />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.PluginConfig}>
                                        <PageWrapper>
                                            <PluginConfig />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.PluginIntegration}>
                                        <PageWrapper>
                                            <PluginIntegration />
                                        </PageWrapper>
                                    </Route>
                                    <Route path={Url.Usage}>
                                        <PageWrapper>
                                            <AnalyticsUsage />
                                        </PageWrapper>
                                    </Route>
                                </Switch>
                            </main>
                            <Footer />
                        </div>
                    </div>
                </ThemeProvider>
            </BrowserRouter>
        )
    }
}

export default withAuthenticator(withStyles(styles)(App), false, undefined, undefined, undefined, undefined, ['email'])
