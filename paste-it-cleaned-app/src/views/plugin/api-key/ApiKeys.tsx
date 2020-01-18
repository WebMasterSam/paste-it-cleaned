import 'date-fns'

import React, { Fragment } from 'react'
import moment from 'moment'
import i18n from 'i18next'

import DateFnsUtils from '@date-io/date-fns'
import VpnKeyIcon from '@material-ui/icons/VpnKey'
import AddIcon from '@material-ui/icons/Add'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import { Skeleton } from '@material-ui/lab'
import MuiAlert from '@material-ui/lab/Alert'
import { MuiPickersUtilsProvider, KeyboardDatePicker } from '@material-ui/pickers'

import { Paper, Typography, Grid, Chip, Button, TextField } from '@material-ui/core'

import { ApiKeyEntity, DomainEntity } from '../../../entities/api'
import { ApiKeysController } from './ApiKeysController'
import ButtonWithLoading from '../../../components/forms/ButtonWithLoading'
import LoadingError from '../../../components/forms/LoadingError'
import AddModal from '../../../components/modals/AddModal'

import ConfirmationModal from '../../../components/modals/ConfirmationModal'
import Snack from '../../../components/forms/Snack'
import UpdateModal from '../../../components/modals/UpdateModal'

import './ApiKeys.less'

export interface ApiKeysProps {}
export interface ApiKeysState {
    isLoaded: boolean
    apiKeys: ApiKeyEntity[]
    apiKeysLoading: boolean
    apiKeysError: boolean
    apiKeyLoading: boolean
    apiKeyError: boolean
    modalAddDomain: {
        visible: boolean
        keyEntity?: ApiKeyEntity
        domainName: string
        error: boolean
    }
    modalDeleteDomain: {
        visible: boolean
        keyEntity?: ApiKeyEntity
        domainEntity?: DomainEntity
    }
    modalDeleteApiKey: {
        visible: boolean
        keyEntity?: ApiKeyEntity
    }
    modalUpdateApiKey: {
        visible: boolean
        keyEntity?: ApiKeyEntity
    }
    successMessage: {
        message: string
        visible: boolean
    }
    errorMessage: {
        message: string
        visible: boolean
    }
}

class ApiKeys extends React.Component<ApiKeysProps, ApiKeysState> {
    private controller?: ApiKeysController = undefined

    constructor(props: ApiKeysProps) {
        super(props)
        this.controller = new ApiKeysController(this)
        this.state = {
            isLoaded: false,
            apiKeys: [],
            apiKeysLoading: false,
            apiKeysError: false,
            apiKeyLoading: false,
            apiKeyError: false,
            modalAddDomain: {
                visible: false,
                domainName: '',
                error: false,
            },
            modalDeleteDomain: {
                visible: false,
            },
            modalDeleteApiKey: {
                visible: false,
            },
            modalUpdateApiKey: {
                visible: false,
            },
            successMessage: {
                message: '',
                visible: false,
            },
            errorMessage: {
                message: '',
                visible: false,
            },
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <Fragment>
                <Typography variant="h2" className="override-h2" component="h2">
                    <VpnKeyIcon /> {i18n.t('views.apiKeys.title')}
                </Typography>
                <br />
                {this.state.apiKeysError ? (
                    <React.Fragment>
                        <LoadingError />
                    </React.Fragment>
                ) : (
                    <React.Fragment>
                        {this.state.apiKeys.length < 10 ? (
                            <ButtonWithLoading loading={this.state.apiKeyLoading} variant="contained" color="primary" onClick={this.controller!.createApiKey}>
                                {i18n.t('views.apiKeys.generateNewKey')}
                            </ButtonWithLoading>
                        ) : (
                            <React.Fragment>
                                <p>{i18n.t('views.apiKeys.generateNewKey')}</p>
                                <br />
                            </React.Fragment>
                        )}

                        {this.state.apiKeysLoading ? (
                            <React.Fragment>
                                <Skeleton height={200} className="paper paper-no-padding" />
                                <Skeleton height={200} className="paper paper-no-padding" />
                            </React.Fragment>
                        ) : (
                            <React.Fragment>
                                {this.state.apiKeys &&
                                    this.state.apiKeys.map(key => (
                                        <Paper className="paper paper-no-padding" key={key.apiKeyId!}>
                                            <Grid container spacing={1}>
                                                <Grid item xs={12} md={2} className="api-key-card-left grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item>
                                                            {moment(key.expiresOn!) < moment() ? (
                                                                <Chip size="small" label={i18n.t('common.expired')} className="chip-red" />
                                                            ) : key.domains!.length > 0 ? (
                                                                <Chip size="small" label={i18n.t('common.active')} className="chip-green" />
                                                            ) : (
                                                                <Chip size="small" label={i18n.t('common.inactive')} />
                                                            )}
                                                        </Grid>
                                                        <Grid item>
                                                            <Typography variant="caption" className="override-caption" component="span" style={{ color: '#ddd' }}>
                                                                {i18n.t('views.apiKeys.expiresOn')}{' '}
                                                                <span style={{ whiteSpace: 'nowrap' }}>
                                                                    {moment(key.expiresOn!).format('YYYY-MM-DD')} <EditIcon onClick={() => this.controller!.showUpdateApiKey({ ...key })} cursor="pointer" />
                                                                </span>
                                                            </Typography>
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                                <Grid item xs={12} md={10} className="grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item style={{ height: '35px', width: '100%' }}>
                                                            <Grid container justify="space-between" alignItems="flex-start" style={{ width: '100%', display: 'flex' }}>
                                                                <Grid item style={{ verticalAlign: 'middle' }}>
                                                                    <Typography variant="h3" className="override-h2" component="h3">
                                                                        <VpnKeyIcon />
                                                                        {key.key}
                                                                    </Typography>
                                                                </Grid>
                                                                <Grid item style={{ alignSelf: 'flex-start', paddingRight: '5px' }}>
                                                                    <Button color="secondary" size="small" onClick={() => this.controller!.showDeleteApiKey(key)}>
                                                                        <DeleteIcon />
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Grid>
                                                        <Grid item>
                                                            {moment(key.expiresOn!) > moment() ? (
                                                                key.domains!.length > 0 ? (
                                                                    key.domains!.map(d => (
                                                                        <Chip
                                                                            key={d.domainId!}
                                                                            size="small"
                                                                            variant="default"
                                                                            color="primary"
                                                                            label={d.name!}
                                                                            className="chip-spaced"
                                                                            deleteIcon={<DeleteIcon />}
                                                                            onDelete={() => this.controller!.showDeleteDomain(key, d)}
                                                                        />
                                                                    ))
                                                                ) : (
                                                                    <span style={{ color: '#f44336' }}>{i18n.t('views.apiKeys.pleaseAddDomain')}</span>
                                                                )
                                                            ) : key.domains!.length > 0 ? (
                                                                key.domains!.map(d => <Chip key={d.domainId!} size="small" variant="default" style={{ color: '#fff', backgroundColor: '#f44336' }} label={d.name!} className="chip-spaced" />)
                                                            ) : (
                                                                <span style={{ color: '#f44336' }}>{i18n.t('views.apiKeys.cannotAddToExpired')}</span>
                                                            )}
                                                        </Grid>
                                                        <Grid item style={{ height: '45px', display: 'flex' }}>
                                                            {moment(key.expiresOn!) > moment() && (
                                                                <Button color="primary" size="small" variant="outlined" style={{ alignSelf: 'flex-end' }} onClick={() => this.controller!.showAddDomain(key)}>
                                                                    <AddIcon /> {i18n.t('views.apiKeys.addDomain')}
                                                                </Button>
                                                            )}
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                        </Paper>
                                    ))}
                            </React.Fragment>
                        )}
                    </React.Fragment>
                )}

                <AddModal
                    title={i18n.t('views.apiKeys.modals.addDomain.title')}
                    open={this.state.modalAddDomain.visible}
                    onCancel={this.controller!.hideAddDomain}
                    onConfirm={() => {
                        this.controller!.createDomain(this.state.modalAddDomain.keyEntity!.apiKeyId!, this.state.modalAddDomain.domainName)
                        this.controller!.hideAddDomain()
                    }}
                    confirmActive={!this.state.modalAddDomain.error && this.state.modalAddDomain.domainName.length > 0}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">
                            {i18n.t('views.apiKeys.apiKey')} <b>{this.state.modalAddDomain.keyEntity && this.state.modalAddDomain.keyEntity!.key}</b>
                        </MuiAlert>
                        <p>{i18n.t('views.apiKeys.modals.addDomain.enterDomain')}</p>
                        <p>{i18n.t('views.apiKeys.modals.addDomain.note')}</p>
                        <TextField
                            onChange={this.controller!.handleAddDomainTyping}
                            name="domain"
                            type="text"
                            label={i18n.t('views.apiKeys.modals.addDomain.domainName')}
                            fullWidth
                            value={this.state.modalAddDomain.domainName}
                            error={this.state.modalAddDomain.error}
                            helperText={this.state.modalAddDomain.error && i18n.t('views.apiKeys.modals.addDomain.domainNameError')}
                        />
                    </React.Fragment>
                </AddModal>

                <MuiPickersUtilsProvider utils={DateFnsUtils}>
                    <UpdateModal
                        title={i18n.t('views.apiKeys.modals.updateApiKey.title')}
                        open={this.state.modalUpdateApiKey.visible}
                        onCancel={this.controller!.hideUpdateApiKey}
                        onConfirm={() => {
                            this.controller!.updateApiKey(this.state.modalUpdateApiKey.keyEntity!)
                            this.controller!.hideUpdateApiKey()
                        }}
                        confirmActive={true}
                    >
                        <React.Fragment>
                            <MuiAlert severity="info">
                                {i18n.t('views.apiKeys.apiKey')} <b>{this.state.modalUpdateApiKey.keyEntity && this.state.modalUpdateApiKey.keyEntity!.key}</b>
                            </MuiAlert>
                            <p>{i18n.t('views.apiKeys.modals.updateApiKey.pleaseSelect')}</p>
                            {this.state.modalUpdateApiKey.keyEntity && (
                                <KeyboardDatePicker
                                    margin="normal"
                                    id="date-picker-dialog"
                                    label={i18n.t('views.apiKeys.modals.updateApiKey.expirationDate')}
                                    format="yyyy-MM-dd"
                                    value={this.state.modalUpdateApiKey.keyEntity!.expiresOn}
                                    onChange={this.controller!.handleUpdateApiKeyExpiresOn}
                                    fullWidth
                                    KeyboardButtonProps={{
                                        'aria-label': 'change date',
                                    }}
                                />
                            )}
                        </React.Fragment>
                    </UpdateModal>
                </MuiPickersUtilsProvider>

                <ConfirmationModal
                    title={i18n.t('views.apiKeys.modals.deleteApiKey.title')}
                    open={this.state.modalDeleteApiKey.visible}
                    onCancel={this.controller!.hideDeleteApiKey}
                    onConfirm={() => {
                        this.controller!.deleteApiKey(this.state.modalDeleteApiKey.keyEntity!.apiKeyId!)
                        this.controller!.hideDeleteApiKey()
                    }}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">
                            {i18n.t('views.apiKeys.apiKey')} <b>{this.state.modalDeleteApiKey.keyEntity && this.state.modalDeleteApiKey.keyEntity!.key}</b>
                        </MuiAlert>
                        <p>{i18n.t('views.apiKeys.modals.deleteApiKey.areYouSure')}</p>
                        <p style={{ color: 'red' }}>{i18n.t('views.apiKeys.modals.common.associatedWillStopWorking')}</p>
                    </React.Fragment>
                </ConfirmationModal>

                <ConfirmationModal
                    title={i18n.t('views.apiKeys.modals.deleteDomain.title')}
                    open={this.state.modalDeleteDomain.visible}
                    onCancel={this.controller!.hideDeleteDomain}
                    onConfirm={() => {
                        this.controller!.deleteDomain(this.state.modalDeleteDomain.keyEntity!.apiKeyId!, this.state.modalDeleteDomain.domainEntity!.domainId!)
                        this.controller!.hideDeleteDomain()
                    }}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">
                            {i18n.t('views.apiKeys.apiKey')} <b>{this.state.modalDeleteDomain.keyEntity && this.state.modalDeleteDomain.keyEntity!.key}</b>
                            <br />
                            {i18n.t('views.apiKeys.domain')} <b>{this.state.modalDeleteDomain.domainEntity && this.state.modalDeleteDomain.domainEntity!.name}</b>
                        </MuiAlert>
                        <p>{i18n.t('views.apiKeys.modals.deleteDomain.areYouSure')}</p>
                        <p style={{ color: 'red' }}>{i18n.t('views.apiKeys.modals.common.associatedWillStopWorking')}</p>
                    </React.Fragment>
                </ConfirmationModal>

                <Snack type="success" visible={this.state.successMessage.visible} onClose={this.controller!.hideSuccessSnackbar}>
                    {this.state.successMessage.message}
                </Snack>

                <Snack type="error" visible={this.state.errorMessage.visible} onClose={this.controller!.hideErrorSnackbar}>
                    {this.state.errorMessage.message}
                </Snack>
            </Fragment>
        )
    }
}

export default ApiKeys
